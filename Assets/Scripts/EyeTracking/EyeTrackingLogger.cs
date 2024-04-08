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

        private DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        private void Start()
        {
            fullFolder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;
            filename = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";
            fullpath = Path.Combine(fullFolder, filename);

            print($"[Eyetracking] Use Log: {fullpath}");
        }

        public void WriteLog(Transform eye, GameObject go)
        {
            var name = go.name;
            var tag = go.tag;

            // refer:
            //ID	TimestampIn	timestampInABS	TimeStampInUnix	ObjName	RelatedX	RelatedY	RelatedZ	RelatedDist	X	Y	Z

            // implemented
            // real-time, time-after-start(second), unixtime (millisecond), name, tag, rel_x, rel_y, rel_z, rel_dist, x, y, z

            // time in real
            var realTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            var rel = Vector3.zero;
            var relDist = 0f;

            var xyz = go.transform.position;

            // Get the time in seconds since the start of the game
            float timeAfterStart = Time.time;

            // Convert the UTC time to milliseconds since the Unix epoch (January 1, 1970)
            long unixTime = (long)(DateTime.UtcNow - unixStart).TotalMilliseconds;

            if (eye != null)
            {
                rel = go.transform.position - eye.position;
                relDist = Vector3.Distance(go.transform.position, eye.position);
            }

            using (TextWriter writer = File.AppendText(fullpath))
            {
                // write text
                writer.WriteLine($"{realTime},{timeAfterStart},{unixTime}," +
                                 $"{name},{tag}," +
                                 $"{rel.x.ToString("f5")},{rel.y.ToString("f5")},{rel.z.ToString("f5")}," +
                                 $"{relDist.ToString("f5")}," +
                                 $"{xyz.x.ToString("f5")},{xyz.y.ToString("f5")},{xyz.z.ToString("f5")}");
            }
        }
    }
}