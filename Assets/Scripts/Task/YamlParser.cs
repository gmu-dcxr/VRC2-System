using System.Collections.Generic;
using JetBrains.Annotations;

namespace VRC2.Task
{
    public class YamlParser
    {
        #region Info

        public class Info
        {
            public int id { get; set; }
            public int segment { get; set; }
            public string color { get; set; }
            public string type { get; set; }
            public int size { get; set; }
            public float length { get; set; }
        }

        #endregion

        #region Task

        public class Task
        {
            public string name { get; set; }
            [CanBeNull] public string desc { get; set; }
            public string folder { get; set; }
            public string image { get; set; }
            public List<string> rule { get; set; }
            public List<string> P1 { get; set; }
            public List<string> P2 { get; set; }
            public List<Info> info { get; set; }
        }

        #endregion
    }
}