using System;
using System.IO;
using UnityEngine;
using VRC2.ScenariosV2.Tool;

namespace VRC2.Loggers
{
    public class LoggerBase : MonoBehaviour
    {
        [ReadOnly] public string folder = "../logger";

        [ReadOnly] public string fullFolder;
        [ReadOnly] public string filename;
        [ReadOnly] public string fullpath;

        private DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void InitConfig(string folder)
        {
            this.folder = folder;

            fullFolder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;
            filename = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";
            fullpath = Path.Combine(fullFolder, filename);
            print($"[LoggerBase] Init log file: {fullpath}");
        }

        string GetRealTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        string GetTimeAfterStart()
        {
            return $"{Time.time}";
        }

        String GetUnixTime()
        {
            long unixTime = (long)(DateTime.UtcNow - unixStart).TotalMilliseconds;
            return $"{unixTime}";
        }

        public void WriteLog(string data)
        {
            var realTime = GetRealTime();
            var timeAfterStart = GetTimeAfterStart();
            var unixTime = GetUnixTime();
            using (TextWriter writer = File.AppendText(fullpath))
            {
                // write text
                writer.WriteLine($"{realTime},{timeAfterStart},{unixTime},{data}");
            }
        }
    }
}