using UnityEngine;
using System.IO;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using VRC2.Record;
using VRC2.Record.Wave;

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
            string filename = string.Format("{0}.log", System.DateTime.Now.ToString("yyyy-MM-dd"));
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), VideoLogWriter.folder, filename);
        }

        public static void Write(string text)
        {
            using (StreamWriter writer = File.AppendText(GetFilename()))
            {
                writer.Write(text);
                writer.Write('\n');
            }
        }
    }

    public class QuestionSeparator
    {
        private string path = "Assets/Resources/Audio/Ding-dong.wav";
        private Wave.Metadata _metadata;

        private byte[] data;

        private string filename;

        private const int HEADER_SIZE = 44; // Size of the .wav file header in bytes

        public QuestionSeparator()
        {
            filename = Path.Combine(Path.GetDirectoryName(Application.dataPath), path);
            data = File.ReadAllBytes(filename);
        }

        public void GetMetaData()
        {
            _metadata = Wave.Reader.GetMetadata(filename);
            Debug.Log(_metadata.GetAllMetadataAsString());
        }

        public byte[] GetData()
        {
            return data;
        }

        public int GetOffset()
        {
            return HEADER_SIZE;
        }

        public int GetLength()
        {
            // 547692
            // var tail = 1e5;
            // return data.Length - HEADER_SIZE;
            // remove some tail noise
            return 400000;
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

        private OutgoingStreamSaverShort osSaverShort;
        private OutgoingStreamSaverFloat osSaverFloat;

        private string outputFolder = "VoiceRecordings";

        private QuestionSeparator _questionSeparator;

        private void PhotonVoiceCreated(PhotonVoiceCreatedParams photonVoiceCreatedParams)
        {
            Debug.Log($"PhotonVoiceCreated");

            voiceInfo = photonVoiceCreatedParams.Voice.Info;
            if (photonVoiceCreatedParams.Voice is LocalVoiceAudioFloat)
            {
                print("Format LocalVoiceAudioFloat");
                _audioFormat = AudioFormat.Float;

            }
            else if (photonVoiceCreatedParams.Voice is LocalVoiceAudioShort)
            {
                print("Format LocalVoiceAudioShort");
                _audioFormat = AudioFormat.Short;
            }

            _localVoice = photonVoiceCreatedParams.Voice;
        }

        public void StartRecording(string type)
        {
            if (_questionSeparator == null)
            {
                _questionSeparator = new QuestionSeparator();
            }

            var filePath = GetFilePath();
            print(
                $"StartRecording [Local] [{type}]: {filePath} SampleRate: {voiceInfo.SamplingRate} Channels: {voiceInfo.Channels}");
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

                // this.wavWriter.Write(_questionSeparator.GetbData(), 44, _questionSeparator.GetbData().Length - 44);

                this.Logger.LogInfo("Outgoing 32 bit stream {0}, output file path: {1}", voiceInfo, filePath);
                LocalVoiceAudioFloat localVoiceAudioFloat = _localVoice as LocalVoiceAudioFloat;
                this.osSaverFloat = new OutgoingStreamSaverFloat(this.wavWriter);
                localVoiceAudioFloat.AddPostProcessor(this.osSaverFloat);
            }
            else if (_audioFormat == AudioFormat.Short)
            {
                this.wavWriter = new WaveWriter(filePath, voiceInfo.SamplingRate, 16, voiceInfo.Channels);
                this.Logger.LogInfo("Outgoing 16 bit stream {0}, output file path: {1}", voiceInfo, filePath);
                LocalVoiceAudioShort localVoiceAudioShort = _localVoice as LocalVoiceAudioShort;
                this.osSaverShort = new OutgoingStreamSaverShort(this.wavWriter);
                localVoiceAudioShort.AddPostProcessor(this.osSaverShort);
            }
        }

        public void InsertSeparator()
        {
            if (_audioFormat == AudioFormat.Float)
            {
                if (this.wavWriter != null)
                {
                    this.wavWriter.Write(_questionSeparator.GetData(),
                        _questionSeparator.GetOffset(),
                        _questionSeparator.GetLength());
                    print("Separator inserted");
                }
            }
        }

        public void StopRecording()
        {
            print("StopRecording");
            if (this.osSaverFloat != null)
            {
                LocalVoiceAudioFloat localVoiceAudioFloat = _localVoice as LocalVoiceAudioFloat;
                localVoiceAudioFloat.RemoveProcessor(this.osSaverFloat);
                this.osSaverFloat.Dispose();
            }

            if (this.osSaverShort != null)
            {
                LocalVoiceAudioShort localVoiceAudioShort = _localVoice as LocalVoiceAudioShort;
                localVoiceAudioShort.RemoveProcessor(this.osSaverShort);
                this.osSaverShort.Dispose();
            }
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
                if (this.wavWriter == null) return null;

                this.wavWriter.WriteSamples(buf, 0, buf.Length);
                return buf;
            }

            public void Dispose()
            {
                this.wavWriter.Dispose();
                this.wavWriter = null;
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
                if (this.wavWriter == null) return null;

                for (int i = 0; i < buf.Length; i++)
                {
                    this.wavWriter.Write(buf[i]);
                }

                return buf;
            }

            public void Dispose()
            {
                this.wavWriter.Dispose();
                this.wavWriter = null;
            }
        }
    }
}