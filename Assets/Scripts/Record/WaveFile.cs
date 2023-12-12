using UnityEngine;
using System;
using System.IO;

namespace VRC2.Record
{
    namespace Wave
    {
        /// <summary>
        /// Metadata of a wave file.
        /// </summary>
        [Serializable]
        public class Metadata
        {
            /// <summary>
            /// Name of the file, including extension.
            /// </summary>
            public string filename;

            /// <summary>
            /// Duration of audio (in seconds).
            /// </summary>
            public float duration;

            /// <summary>
            /// Total file size in bytes.
            /// </summary>
            public uint fileBytes;

            /// <summary>
            /// RIFF type ID. Usually "WAVE".
            /// </summary>
            public string riffTypeID;

            /// <summary>
            /// Compression code. Uncompressed PCM audio will have a value of 1.
            /// </summary>
            public uint compressionCode;

            /// <summary>
            /// Number of audio channels. 1 = Mono, 2 = Stereo.
            /// </summary>
            public uint channelCount;

            /// <summary>
            /// Samples per second.
            /// </summary>
            public uint sampleRate;

            /// <summary>
            /// Average bytes per second. For example, a PCM wave file that has a sampling rate of 44100 Hz, 1 channel, and sampling resolution of 16 bits (2 bytes) per sample, will have an average number of bytes equal to 44100 * 2 * 1 = 88,200.
            /// </summary>
            public uint avgBytesPerSec;

            /// <summary>
            /// Byte-size of sample blocks. For example, a PCM wave that has a sampling resolution of 16 bits (2 bytes) and has 2 channels will record a block of samples in 2 * 2 = 4 bytes.
            /// </summary>
            public uint blockAlign;

            /// <summary>
            /// Significant bits per sample. Defines the sampling resolution of the file. A typical sampling resolution is 16 bits per sample, but could be anything greater than 1.
            /// </summary>
            public uint bitRate;

            /// <summary>
            /// Total number of audio samples.
            /// </summary>
            public uint sampleCount;

            /// <summary>
            /// The maximum sample value. Sample values range between -maxSampleValue and +maxSampleValue. The value depends on the bitrate.
            /// </summary>
            public int maxSampleValue;

            /// <summary>
            /// Cues/markers found in the wave file.
            /// </summary>
            public Cue[] cues;

            /// <summary>
            /// Get all metadata as a formatted string, with linebreaks.
            /// </summary>
            /// <returns></returns>
            public string GetAllMetadataAsString()
            {
                string str;
                str = filename + "\n";
                str += "Duration: " + duration + " s\n";
                str += "Size: " + fileBytes + "\n";
                str += "Riff type ID: " + riffTypeID + "\n";
                str += "Compression code: " + compressionCode + "\n";
                str += "Channel count: " + channelCount + "\n";
                str += "Sample rate: " + sampleRate + "\n";
                str += "Avg bytes per sec: " + avgBytesPerSec + "\n";
                str += "Block align: " + blockAlign + "\n";
                str += "Bitrate: " + bitRate + "\n";
                str += "Sample count: " + sampleCount + "\n";
                // if (cues == null)
                //     str += "No cues";
                // else
                //     str += "Cues:\n";
                // foreach (Cue c in cues)
                // {
                //     str += string.Format(" - ID: {0} - Name: {1} - Position: {2} - dataChunkID: {3}\n", c.ID, c.name,
                //         c.position, c.dataChunkID);
                // }

                return str;
            }
        }

        /// <summary>
        /// A cue from inside the wave-file.
        /// </summary>
        [Serializable]
        public struct Cue
        {
            /// <summary>
            /// Unique index of this cue.
            /// </summary>
            public uint ID;

            /// <summary>
            /// Identifier-string for the cue/marker.
            /// </summary>
            public string name;

            /// <summary>
            /// The sample on which this cue appears within the audio.
            /// </summary>
            public uint position;

            /// <summary>
            /// Either "data" or "slnt" depending on whether the cue occurs in a data chunk or in a silent chunk.
            /// </summary>
            public uint dataChunkID;
        }

        /// <summary>
        /// Reads metadata from a wave file.
        /// </summary>
        public class Reader
        {
            /// <summary>
            /// Get metadata from a given wave file.
            /// </summary>
            /// <param name="path">Path to the file, including extension.</param>
            /// <returns>Metadata object.</returns>
            public static Metadata GetMetadata(string path)
            {
                // Check if file exists
                if (!File.Exists(path))
                {
                    Debug.LogError("Couldn't locate file: " + path);
                    return null;
                }

                // Check filetype
                string ext = Path.GetExtension(path);
                if (!(ext == ".wav" || ext == ".bwf"))
                {
                    Debug.LogWarning("Only extensions .wav and .bwf are supported for reading metadata: " + path);
                    return null;
                }

                Metadata data = new Metadata
                {
                    filename = Path.GetFileName(path)
                };
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                int n = 0;
                // TODO: Change into a for-loop
                while (fs.Position < fs.Length)
                {
                    ReadNextChunk(fs, data);

                    if (n > 999)
                    {
                        Debug.LogError("Cancelled infinite loop upon reading wave file metadata...");
                        break;
                    }

                    n++;
                }

                fs.Close();

                // Calculate duration
                data.duration = (float)data.sampleCount / data.sampleRate;

                // Debug
                //data.Print();

                return data;
            }

            /// <summary>
            /// Reads the next chunk of a wave file. Reference: https://www.recordingblogs.com/wiki/wave-file-format
            /// </summary>
            /// <param name="fs">FileStream object</param>
            /// <param name="data">Metadata object</param>
            static void ReadNextChunk(FileStream fs, Metadata data)
            {
                long initialPos = fs.Position;
                string chunkID = GetString(fs, 4);
                uint chunkSize = GetUInt(fs, 4);
                long chunkEndPos = initialPos + chunkSize + 8;

                //Debug.Log(chunkID);

                switch (chunkID.ToUpper())
                {
                    case "RIFF":
                        data.fileBytes = chunkSize + 8;
                        data.riffTypeID = GetString(fs, 4);
                        break;
                    case "FMT ":
                        data.compressionCode = GetUInt(fs, 2);
                        data.channelCount = GetUInt(fs, 2);
                        data.sampleRate = GetUInt(fs, 4);
                        data.avgBytesPerSec = GetUInt(fs, 4);
                        data.blockAlign = GetUInt(fs, 2);
                        data.bitRate = GetUInt(fs, 2);
                        fs.Position = chunkEndPos; // Go to end of chunk
                        break;
                    case "DATA":
                        data.sampleCount = (chunkSize / (data.bitRate / 8)) / data.channelCount;
                        fs.Position = chunkEndPos; // Go to end of chunk
                        break;
                    case "CUE ":
                        uint cueCount = (GetUInt(fs, 4));
                        data.cues = new Cue[cueCount];

                        // Loop through cues
                        for (int i = 0; i < cueCount; i++)
                        {
                            long p = fs.Position;
                            data.cues[i].ID = GetUInt(fs, 4);
                            data.cues[i].position = GetUInt(fs, 4);
                            data.cues[i].dataChunkID = GetUInt(fs, 4);

                            fs.Position = p + 24; // Skip to next cue
                        }

                        fs.Position = chunkEndPos; // Go to end of chunk
                        break;
                    case "LIST":
                        string listID = GetString(fs, 4).ToUpper();
                        if (listID == "ADTL")
                        {
                            // ADTL = Associated Data List
                            uint remainingBytes = chunkSize - 4;

                            string subChunkID;
                            uint subChunkSize;
                            int cueIndex = 0;

                            while (remainingBytes > 0)
                            {
                                subChunkID = GetString(fs, 4); // labl
                                subChunkSize = GetUInt(fs, 4); // chunk size

                                if (subChunkID.ToUpper() == "LABL" && data.cues != null)
                                {
                                    data.cues[cueIndex].ID = GetUInt(fs, 4);
                                    data.cues[cueIndex].name = GetString(fs, (int)subChunkSize - 4);

                                    remainingBytes -= subChunkSize + 8;

                                    // Check for uneven number of remaining bytes (which means the next byte is an empty padding)
                                    if (remainingBytes % 2 == 1)
                                    {
                                        remainingBytes -= 1;
                                        fs.ReadByte(); // Read the padded byte
                                    }

                                    cueIndex++;
                                }
                                else
                                {
                                    remainingBytes -= subChunkSize;
                                    fs.Seek(subChunkSize, SeekOrigin.Current); // Go to end of subchunk
                                }
                            }
                        }

                        fs.Position = chunkEndPos; // Go to end of chunk
                        break;
                    default:
                        fs.Position = chunkEndPos; // Go to end of chunk
                        break;
                }
            }

            /// <summary>
            /// Read a file and get only the data chunk.
            /// </summary>
            /// <param name="fs">FileStream object reading from a wave file.</param>
            public static int[] GetDataChunk(FileStream fs, int arraySize)
            {
                uint channelCount = 0, bitRate = 0, compressionCode = 0;

                for (int i = 0; i < 99; i++)
                {
                    long initialPos = fs.Position;
                    string chunkID = GetString(fs, 4);
                    uint chunkSize = GetUInt(fs, 4);
                    long chunkEndPos = initialPos + chunkSize + 8;

                    switch (chunkID.ToUpper())
                    {
                        case "RIFF":
                            fs.Position += 4;
                            break;
                        case "FMT ":
                            compressionCode = GetUInt(fs, 2);
                            channelCount = GetUInt(fs, 2);
                            GetUInt(fs, 4); // sample rate
                            GetUInt(fs, 4); // avg bytes per sec
                            GetUInt(fs, 2); // block align
                            bitRate = GetUInt(fs, 2);
                            fs.Position = chunkEndPos; // Go to end of chunk
                            break;
                        case "DATA":
                            if (compressionCode != 1)
                                return null;

                            // Convert byte array to int array
                            uint sampleCount = chunkSize / (channelCount + bitRate / 8);
                            int[] sampleData = new int[arraySize];
                            for (int n = 0; n < sampleData.Length; n++)
                            {
                                int avg = 0;
                                int numberOfCollatedSamples =
                                    ((int)sampleCount / sampleData.Length) * (int)channelCount;
                                for (int m = 0; m < numberOfCollatedSamples; m++)
                                {
                                    avg += Mathf.Abs(GetInt16(fs));
                                }

                                avg = avg / numberOfCollatedSamples;
                                sampleData[n] = avg;
                            }

                            return sampleData;
                        default:
                            fs.Position = chunkEndPos;
                            break;
                    }
                }

                return null;
            }

            public static uint GetUInt(FileStream fs, int byteNum)
            {
                return BitConverter.ToUInt32(ReadBytes(fs, byteNum), 0);
            }

            public static byte GetByte(FileStream fs)
            {
                return ReadBytes(fs, 1)[0];
            }

            public static int GetInt16(FileStream fs)
            {
                return BitConverter.ToInt16(ReadBytes(fs, 2), 0);
            }

            public static int GetInt24(FileStream fs)
            {
                byte[] readBytes = ReadBytes(fs, 3);
                byte[] newBytes = new byte[4];
                newBytes[0] = 0;
                newBytes[1] = readBytes[0];
                newBytes[2] = readBytes[1];
                newBytes[3] = readBytes[2];
                return BitConverter.ToInt32(newBytes, 0);
            }

            public static float GetFloat(FileStream fs)
            {
                return BitConverter.ToSingle(ReadBytes(fs, 4), 0);
            }

            public static int GetInt32(FileStream fs)
            {
                return BitConverter.ToInt32(ReadBytes(fs, 4), 0);
            }

            public static string GetString(FileStream fs, int byteNum)
            {
                return System.Text.Encoding.UTF8.GetString(ReadBytes(fs, byteNum)).Trim('\0');
            }

            public static byte[] ReadBytes(FileStream fs, int num)
            {
                byte[] bytes = new byte[num < 4 ? 4 : num];
                for (int i = 0; i < num; i++)
                {
                    int b = fs.ReadByte();
                    bytes[i] = (byte)b;
                }

                return bytes;
            }
        }

        // Below is an experimental waveform texture generator that lets you read a handful of samples each frame to avoid freezing/stuttering.
        // If you're making a custom inspector, I recommend fetching Unity's native generated waveform instead.
        /*

        public class WaveformTextureGenerator
        {
            FileStream fs;
            uint dataChunkSize = 0, dataSampleCount = 0, channelCount = 0, bitRate = 0, compressionCode = 0;
            long dataStartByte = 0;
            int maxSampleValue;
            Texture2D texture;

            int lastInjectedRow;
            float[] avgValues;
            int[] avgValuesNum;

            Color colWaveform;

            public bool Initialize(string path, int textureWidth, int textureHeight, Color colBackground, Color colWaveform) {
                if (!File.Exists(path)) {
                    Debug.LogWarning("Couldn't locate file: " + path);
                    return false;
                }

                // Check filetype
                string ext = Path.GetExtension(path);
                if (!(ext == ".wav" || ext == ".bwf")) {
                    Debug.LogWarning("Only extensions .wav and .bwf are supported for reading metadata: " + path);
                    return false;
                }

                // Open filestream
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (!fs.CanRead) {
                    Debug.LogWarning("FileStream can't read for some reason...");
                    return false;
                }

                fs.Position = 0;
                ReadMetadata();

                // Find maximum value of each sample
                switch (bitRate) {
                    case 8:
                        maxSampleValue = 255;
                        break;
                    case 16:
                        maxSampleValue = 32767;
                        break;
                    case 24:
                        maxSampleValue = 2147483647; //Normally 8388607, but we need to use 32-bit encoding since there is no native 24-bit int in C#;
                        break;
                    case 32:
                        if (compressionCode == 3)
                            maxSampleValue = 1; // Floating-point
                        else
                            maxSampleValue = 2147483647; // PCM 32-bit integer
                        break;
                    default:
                        Debug.LogWarning("The bitrate of this file is unsupported for waveform reading: " + bitRate + "-bit - " + path);
                        StopReading();
                        return false;
                }

                // Generate default texture
                Color[] pix = new Color[textureWidth * textureHeight];
                for (int i = 0; i < textureHeight; i++) {
                    for (int n = 0; n < textureWidth; n++) {
                        pix[i * textureWidth + n] = colBackground;
                    }
                }
                texture = new Texture2D(textureWidth, textureHeight);
                texture.SetPixels(pix);
                texture.Apply(false);

                lastInjectedRow = -1;
                avgValues = new float[texture.width];
                avgValuesNum = new int[texture.width];
                for (int i = 0; i < texture.width; i++) {
                    avgValues[i] = 0;
                    avgValuesNum[i] = 0;
                }

                this.colWaveform = colWaveform;

                return true;
            }

            public void ReadSamples(uint samplesToRead) {
                if (fs == null) return;
                if (samplesToRead < channelCount) {
                    samplesToRead = channelCount;
                }

                // Get current read position relative to data chunk start
                long relativePosition = fs.Position - dataStartByte;

                // Check if end will be reached
                bool hasReachedEnd = false;
                if (relativePosition + ToBytes(samplesToRead) >= dataChunkSize) {
                    hasReachedEnd = true;
                    samplesToRead = ToSamples(dataChunkSize - (uint)relativePosition);
                }

                // Read and store sample values
                int rowIndex = 0;
                float v = 0;

                switch (bitRate) {
                    case 8:
                        for (int i = 0; i < samplesToRead / channelCount; i++) {
                            for (int n = 0; n < channelCount; n++) {
                                rowIndex = Mathf.FloorToInt(((float)(fs.Position - dataStartByte) / dataChunkSize) * (texture.width - 1));
                                v = Reader.GetByte(fs) - 127;
                                avgValues[rowIndex] += (float)Mathf.Abs(v) / (maxSampleValue / 2);
                                avgValuesNum[rowIndex]++;
                            }
                        }
                        break;
                    case 16:
                        for (int i = 0; i < samplesToRead / channelCount; i++) {
                            for (int n = 0; n < channelCount; n++) {
                                rowIndex = Mathf.FloorToInt(((float)(fs.Position - dataStartByte) / dataChunkSize) * (texture.width - 1));
                                v = Reader.GetInt16(fs);
                                avgValues[rowIndex] += (float)Mathf.Abs(v) / maxSampleValue;
                                avgValuesNum[rowIndex]++;
                            }
                        }
                        break;
                    case 24:
                        for (int i = 0; i < samplesToRead / channelCount; i++) {
                            for (int n = 0; n < channelCount; n++) {
                                rowIndex = Mathf.FloorToInt(((float)(fs.Position - dataStartByte) / dataChunkSize) * (texture.width - 1));
                                v = Reader.GetInt24(fs);
                                avgValues[rowIndex] += (float)Mathf.Abs(v) / maxSampleValue;
                                avgValuesNum[rowIndex]++;
                            }
                        }
                        break;
                    case 32:
                        if (compressionCode == 3) { // Floating-point
                            for (int i = 0; i < samplesToRead / channelCount; i++) {
                                for (int n = 0; n < channelCount; n++) {
                                    rowIndex = Mathf.FloorToInt(((float)(fs.Position - dataStartByte) / dataChunkSize) * (texture.width - 1));
                                    v = Reader.GetFloat(fs);
                                    avgValues[rowIndex] += Mathf.Abs(v) / maxSampleValue;
                                    avgValuesNum[rowIndex]++;
                                }
                            }
                        } else { // PCM 32-bit integers
                            for (int i = 0; i < samplesToRead / channelCount; i++) {
                                for (int n = 0; n < channelCount; n++) {
                                    rowIndex = Mathf.FloorToInt(((float)(fs.Position - dataStartByte) / dataChunkSize) * (texture.width - 1));
                                    v = Reader.GetInt32(fs);
                                    avgValues[rowIndex] += (float)Mathf.Abs(v) / maxSampleValue;
                                    avgValuesNum[rowIndex]++;
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }

                // Check if it's time to inject pixel rows into the texture
                for (int i = lastInjectedRow + 1; i < rowIndex; i++) {
                    InjectRow(i);
                }

                // End
                if (hasReachedEnd) {
                    // Inject the last pixel row
                    InjectRow(texture.width - 1);

                    //Debug.Log("Data was " + dataChunkSize + ". Ended on position " + (fs.Position - dataStartByte));
                    StopReading();
                }
            }

            void InjectRow(int index) {

                // Find average value
                if (avgValuesNum[index] > 0)
                    avgValues[index] = avgValues[index] / avgValuesNum[index];

                // Inject
                InjectSampleValueIntoTexture(index, avgValues[index]);

                // Remember last injected
                lastInjectedRow = index;
            }

            uint ToSamples(uint bytes) {
                return bytes / (bitRate / 8);
            }

            uint ToBytes(uint samples) {
                return samples * (bitRate / 8);
            }

            void DebugText(string str) {
                GameObject.Find("DebugText").GetComponent<UnityEngine.UI.Text>().text = str;
            }

            void InjectSampleValueIntoTexture(int row, float value) {
                int w = texture.width, h = texture.height;

                float displayValue = Mathf.Sqrt(1f - Mathf.Pow(value - 1f, 2f));

                // Zero-line
                texture.SetPixel(row, texture.height / 2, colWaveform);

                // Waveform
                for (int i = 0; i < h; i++) {
                    if ((float)i / h < displayValue / 2f + 0.5f && (float)i / h > 0.5f - displayValue / 2f)
                        texture.SetPixel(row, i, colWaveform);
                }

                texture.Apply(false);
            }

            public void StopReading() {
                if (fs == null) return;

                fs.Close();
                fs = null;
            }

            public bool HasFinished() {
                return (fs == null);
            }

            public Texture2D GetTexture() {
                return texture;
            }

            void ReadMetadata() {
                for (int i = 0; i < 99; i++) {
                    long initialPos = fs.Position;
                    string chunkID = Reader.GetString(fs, 4);
                    uint chunkSize = Reader.GetUInt(fs, 4);
                    long chunkEndPos = initialPos + chunkSize + 8;

                    switch (chunkID.ToUpper()) {
                        case "RIFF":
                            fs.Position += 4;
                            break;
                        case "FMT ":
                            compressionCode = Reader.GetUInt(fs, 2);
                            channelCount = Reader.GetUInt(fs, 2);
                            fs.Position += 10;
                            bitRate = Reader.GetUInt(fs, 2);

                            fs.Position = chunkEndPos;
                            break;
                        case "DATA":
                            dataChunkSize = chunkSize;
                            dataSampleCount = chunkSize / bitRate;
                            dataStartByte = initialPos + 8;
                            fs.Position = dataStartByte;
                            return; // stop here
                        default:
                            fs.Position = chunkEndPos;
                            break;
                    }
                }
            }
        }
        */
    }
}