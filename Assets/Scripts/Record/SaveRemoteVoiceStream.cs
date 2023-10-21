using System;
using UnityEngine;
using System.IO;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;

namespace VRC2.Record
{
    using UnityEngine;
    using System.IO;

    [RequireComponent(typeof(VoiceConnection))]
    [DisallowMultipleComponent]
    public class SaveRemoteVoiceStream : VoiceComponent
    {

        [Header("Monitor")] public VoiceClientMonitor monitor;
        private RemoteVoiceLink supervisorVoiceLink;
        private WaveWriter wavWriter;

        private VoiceConnection voiceConnection;

        [SerializeField] private bool muteLocalSpeaker = false;


        protected override void Awake()
        {
            base.Awake();
            this.voiceConnection = this.GetComponent<VoiceConnection>();
            // this.voiceConnection.RemoteVoiceAdded += this.OnRemoteVoiceAdded;
            this.voiceConnection.SpeakerLinked += this.OnSpeakerLinked;
        }

        private void Start()
        {
            monitor.OnVoiceLinkDetermined += OnVoiceLinkDetermined;
        }

        private void OnVoiceLinkDetermined()
        {
            print(
                $"OnVoiceLinkDetermined: Supervisor {monitor.supervisorVoiceLink.PlayerId} {monitor.supervisorVoiceLink.VoiceId}");
            supervisorVoiceLink = monitor.supervisorVoiceLink;

            supervisorVoiceLink.FloatFrameDecoded += SupervisorVoiceLinkOnFloatFrameDecoded;
            supervisorVoiceLink.RemoteVoiceRemoved += SupervisorVoiceLinkOnRemoteVoiceRemoved;
        }

        private void SupervisorVoiceLinkOnRemoteVoiceRemoved()
        {
            this.Logger.LogInfo("Remote voice stream removed: Saving wav file.");
            wavWriter.Dispose();
        }

        private void SupervisorVoiceLinkOnFloatFrameDecoded(FrameOut<float> f)
        {
            if (wavWriter != null)
            {
                wavWriter.WriteSamples(f.Buf, 0, f.Buf.Length);
            }
        }

        private void OnSpeakerLinked(Speaker speaker)
        {
            if (this.muteLocalSpeaker && speaker.RemoteVoice.PlayerId == voiceConnection.Client.LocalPlayer.ActorNumber)
            {
                AudioSource audioSource = speaker.GetComponent<AudioSource>();
                audioSource.mute = true;
                audioSource.volume = 0f;
            }
        }

        private void OnDestroy()
        {
            // this.voiceConnection.RemoteVoiceAdded -= this.OnRemoteVoiceAdded;
        }

        // private void OnRemoteVoiceAdded(RemoteVoiceLink remoteVoiceLink)
        // {
        //     int bitsPerSample = 32;
        //     string filePath = this.GetFilePath(remoteVoiceLink);
        //     Debug.Log($"Write voice recording to: {filePath}");
        //     this.Logger.LogInfo("Incoming stream {0}, output file path: {1}", remoteVoiceLink.VoiceInfo, filePath);
        //     WaveWriter waveWriter = new WaveWriter(filePath, remoteVoiceLink.VoiceInfo.SamplingRate, bitsPerSample,
        //         remoteVoiceLink.VoiceInfo.Channels);
        //     remoteVoiceLink.FloatFrameDecoded += f => { waveWriter.WriteSamples(f.Buf, 0, f.Buf.Length); };
        //     remoteVoiceLink.RemoteVoiceRemoved += () =>
        //     {
        //         this.Logger.LogInfo("Remote voice stream removed: Saving wav file.");
        //         waveWriter.Dispose();
        //     };
        // }

        private string GetFilePath(RemoteVoiceLink remoteVoiceLink)
        {
            string filename = string.Format("in_{0}_{1}_{2}_{3}_{4}.wav",
                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffff"), Random.Range(0, 1000),
                remoteVoiceLink.ChannelId, remoteVoiceLink.PlayerId, remoteVoiceLink.VoiceId);

            return Path.Combine(Path.GetDirectoryName(Application.dataPath), "VoiceRecordings", filename);
            // return Path.Combine(Application.persistentDataPath, filename);
        }

        public void StartRecording()
        {
            var filePath = this.GetFilePath(supervisorVoiceLink);
            print($"StartRecording [Remote]: {filePath}");
            // close previous one
            if (this.wavWriter != null)
            {
                this.wavWriter.Dispose();
            }

            int bitsPerSample = 32;
            this.Logger.LogInfo("Incoming stream {0}, output file path: {1}", supervisorVoiceLink.VoiceInfo, filePath);

            wavWriter = new WaveWriter(filePath, supervisorVoiceLink.VoiceInfo.SamplingRate, bitsPerSample,
                supervisorVoiceLink.VoiceInfo.Channels);
        }

        public void StopRecording()
        {
            this.wavWriter.Dispose();
            this.wavWriter = null;
        }
    }
}

