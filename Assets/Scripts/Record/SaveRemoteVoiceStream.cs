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
        private RemoteVoiceLink safetyManagerVoiceLink;
        private WaveWriter supervisorWriter;
        private WaveWriter safetyManagerWriter;

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
            print(
                $"OnVoiceLinkDetermined: SafetyManager {monitor.safetyManagerVoiceLink.PlayerId} {monitor.safetyManagerVoiceLink.VoiceId}");
            supervisorVoiceLink = monitor.supervisorVoiceLink;
            safetyManagerVoiceLink = monitor.safetyManagerVoiceLink;

            supervisorVoiceLink.FloatFrameDecoded += SupervisorVoiceLinkOnFloatFrameDecoded;
            supervisorVoiceLink.RemoteVoiceRemoved += SupervisorVoiceLinkOnRemoteVoiceRemoved;

            safetyManagerVoiceLink.FloatFrameDecoded += SafetyManagerVoiceLinkOnFloatFrameDecoded;
            safetyManagerVoiceLink.RemoteVoiceRemoved += SafetyManagerVoiceLinkOnRemoteVoiceRemoved;
        }

        private void SafetyManagerVoiceLinkOnRemoteVoiceRemoved()
        {
            this.Logger.LogInfo("Remote voice stream removed: Saving wav file.");
            safetyManagerWriter.Dispose();
        }

        private void SafetyManagerVoiceLinkOnFloatFrameDecoded(FrameOut<float> f)
        {
            if (safetyManagerWriter != null)
            {
                safetyManagerWriter.WriteSamples(f.Buf, 0, f.Buf.Length);
            }
        }

        private void SupervisorVoiceLinkOnRemoteVoiceRemoved()
        {
            this.Logger.LogInfo("Remote voice stream removed: Saving wav file.");
            supervisorWriter.Dispose();
        }

        private void SupervisorVoiceLinkOnFloatFrameDecoded(FrameOut<float> f)
        {
            if (supervisorWriter != null)
            {
                supervisorWriter.WriteSamples(f.Buf, 0, f.Buf.Length);
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

        public void StartRecording(string type, bool supervisor)
        {
            var filePath = this.GetFilePath(safetyManagerVoiceLink);

            if (supervisor)
            {
                filePath = this.GetFilePath(supervisorVoiceLink);

                // close previous one
                if (this.supervisorWriter != null)
                {
                    this.supervisorWriter.Dispose();
                }

                this.Logger.LogInfo("Incoming stream {0}, output file path: {1}", supervisorVoiceLink.VoiceInfo,
                    filePath);
            }
            else
            {
                // close previous one
                if (this.safetyManagerWriter != null)
                {
                    this.safetyManagerWriter.Dispose();
                }

                this.Logger.LogInfo("Incoming stream {0}, output file path: {1}", safetyManagerVoiceLink.VoiceInfo,
                    filePath);
            }

            print($"StartRecording [Remote] [{type}]: {filePath}");
            // write log
            VideoLogWriter.Write($"{type}, {filePath}");

            int bitsPerSample = 32;

            if (supervisor)
            {
                supervisorWriter = new WaveWriter(filePath, supervisorVoiceLink.VoiceInfo.SamplingRate, bitsPerSample,
                    supervisorVoiceLink.VoiceInfo.Channels);
            }
            else
            {
                safetyManagerWriter = new WaveWriter(filePath, safetyManagerVoiceLink.VoiceInfo.SamplingRate,
                    bitsPerSample,
                    safetyManagerVoiceLink.VoiceInfo.Channels);
            }
        }

        public void StopRecording()
        {
            this.safetyManagerWriter.Dispose();
            this.safetyManagerWriter = null;
            this.supervisorWriter.Dispose();
            this.supervisorWriter = null;
        }
    }
}

