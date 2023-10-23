using UnityEngine;
using System.IO;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;

namespace VRC2.Record
{

    enum AudioFormat
    {
        Unknown,
        Float,
        Short
    }

    public class VideoLogWriter
    {
        private static string folder = "VoiceRecordings";
        public static string GetFilename()
        {
            string filename = string.Format("{0}.log",System.DateTime.Now.ToString("yyyy-MM-dd"));   
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), VideoLogWriter.folder, filename);
        }
        public static void Write(string text)
        {
            using (var writer = new StreamWriter(GetFilename()))
            {
                writer.Write(text);
                writer.Write('\n');
            }
        }
    }
    

    [RequireComponent(typeof(Recorder))]
    [DisallowMultipleComponent]
    public class SaveLocalVoiceStream : VoiceComponent
    {
        private WaveWriter wavWriter;

        private AudioFormat _audioFormat = AudioFormat.Unknown;

        private LocalVoice _localVoice;
        private VoiceInfo voiceInfo;

        private string outputFolder = "VoiceRecordings";

        private void PhotonVoiceCreated(PhotonVoiceCreatedParams photonVoiceCreatedParams)
        {
            Debug.Log($"PhotonVoiceCreated");

            voiceInfo = photonVoiceCreatedParams.Voice.Info;
            if (photonVoiceCreatedParams.Voice is LocalVoiceAudioFloat)
            {
                _audioFormat = AudioFormat.Float;

            }
            else if (photonVoiceCreatedParams.Voice is LocalVoiceAudioShort)
            {
                _audioFormat = AudioFormat.Short;
            }

            _localVoice = photonVoiceCreatedParams.Voice;
        }

        public void StartRecording(string type)
        {
            var filePath = GetFilePath();
            print($"StartRecording [Local] [{type}]: {filePath}");
            // write log
            VideoLogWriter.Write($"{type}, {filePath}");
            // close previous one
            if (this.wavWriter != null)
            {
                this.wavWriter.Dispose();
            }

            // create new writer
            if (_audioFormat == AudioFormat.Float)
            {
                this.wavWriter = new WaveWriter(filePath, voiceInfo.SamplingRate, 32, voiceInfo.Channels);

                this.Logger.LogInfo("Outgoing 32 bit stream {0}, output file path: {1}", voiceInfo, filePath);
                LocalVoiceAudioFloat localVoiceAudioFloat = _localVoice as LocalVoiceAudioFloat;
                localVoiceAudioFloat.AddPostProcessor(new OutgoingStreamSaverFloat(this.wavWriter));
            }
            else if (_audioFormat == AudioFormat.Short)
            {
                this.wavWriter = new WaveWriter(filePath, voiceInfo.SamplingRate, 16, voiceInfo.Channels);
                this.Logger.LogInfo("Outgoing 16 bit stream {0}, output file path: {1}", voiceInfo, filePath);
                LocalVoiceAudioShort localVoiceAudioShort = _localVoice as LocalVoiceAudioShort;
                localVoiceAudioShort.AddPostProcessor(new OutgoingStreamSaverShort(this.wavWriter));
            }
        }

        public void StopRecording()
        {
            print("StopRecording");
            this.wavWriter.Dispose();
            this.wavWriter = null;
        }


        private string GetFilePath()
        {
            string filename = string.Format("out_{0}_{1}.wav",
                System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffff"),
                Random.Range(0, 1000));
            // return Path.Combine(Application.persistentDataPath, filename);
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), this.outputFolder, filename);
        }

        private void PhotonVoiceRemoved()
        {
            this.wavWriter.Dispose();
            this.wavWriter = null;
            this.Logger.LogInfo("Recording stopped: Saving wav file.");
        }

        class OutgoingStreamSaverFloat : IProcessor<float>
        {
            private WaveWriter wavWriter;

            public OutgoingStreamSaverFloat(WaveWriter waveWriter)
            {
                this.wavWriter = waveWriter;
            }

            public float[] Process(float[] buf)
            {
                this.wavWriter.WriteSamples(buf, 0, buf.Length);
                return buf;
            }

            public void Dispose()
            {
                this.wavWriter.Dispose();
            }
        }

        class OutgoingStreamSaverShort : IProcessor<short>
        {
            private WaveWriter wavWriter;

            public OutgoingStreamSaverShort(WaveWriter waveWriter)
            {
                this.wavWriter = waveWriter;
            }

            public short[] Process(short[] buf)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    this.wavWriter.Write(buf[i]);
                }

                return buf;
            }

            public void Dispose()
            {
                this.wavWriter.Dispose();
            }
        }
    }
}