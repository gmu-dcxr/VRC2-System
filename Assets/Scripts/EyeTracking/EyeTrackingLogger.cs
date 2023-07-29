using System;
using System.IO;
using UnityEngine;

namespace VRC2
{
    public class EyeTrackingLogger : MonoBehaviour
    {
        public string folder = "../Eyetracking";

        private string fullFolder;
        private string filename;
        private string fullpath;

        private void Start()
        {
            fullFolder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;
            filename = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";
            fullpath = Path.Combine(fullFolder, filename);

            print($"[Eyetracking] Use Log: {fullpath}");
        }

        public void WriteLog(GameObject go)
        {
            var name = go.name;
            var tag = go.tag;

            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            using (TextWriter writer = File.AppendText(fullpath))
            {
                // write text
                writer.WriteLine($"{time},{name},{tag}");
            }
        }
    }
}