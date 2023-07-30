using System;
using System.IO;

namespace VRC2.Scenarios
{
    public static class Helper
    {
        public static char timeSep = '~';
        private static DateTime startPoint = new DateTime(2023, 1, 1);

        public static string GetConfigureFile(string root, string name)
        {
            // var dir = Directory.CreateDirectory(Path.Combine(root, "../Conf")).FullName;
            return Path.Combine(root, $"./Conf/{name}");
        }

        public static int SecondNow()
        {
            return (int)DateTimeOffset.UtcNow.Subtract(startPoint).TotalSeconds;
        }

        public static int TimeToSecond(string time)
        {
            var sep = ':';
            var arr = time.Split(sep);
            var minute = int.Parse(arr[0]);
            var second = int.Parse(arr[1]);
            return minute * 60 + second;
        }

        public static void ParseTime(string t, ref int startInSec, ref int endInSec)
        {
            if (t.Contains(timeSep.ToString()))
            {
                var arr = t.Split(timeSep);
                var start = arr[0];
                var end = arr[1];

                startInSec = TimeToSecond(start);
                endInSec = TimeToSecond(end);
            }
            else
            {
                var sec = TimeToSecond(t);
                startInSec = sec;
                endInSec = -1;
            }
        }

        public static string GetIncidentCallbackName(string name, int incident, ScenarioCallback type)
        {
            var t = Utils.GetDisplayName<ScenarioCallback>(type);

            return $"On_{name}_{incident}_{t}";
        }

        public static string GetScenarioCallbackName(int _id, ScenarioCallback type)
        {
            var t = Utils.GetDisplayName<ScenarioCallback>(type);

            return $"On_Scenario_{_id}_{t}";
        }

        #region Audio File

        public static string GetAudioFileName(string scenario, int incidentid, bool context, bool amount)
        {
            var name = $"{scenario}_{incidentid}";
            if (context) name += "_C";
            if (amount) name += "_A";
            name += ".wav";
            return name;
        }

        #endregion
    }
}