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
            [CanBeNull] public string color { get; set; }
            [CanBeNull] public string type { get; set; }
            [CanBeNull] public int? size { get; set; }
            [CanBeNull] public float? length { get; set; }

            public Info()
            {
                // construct as null for not id and segment
                id = 0;
                segment = 0;
                color = null;
                type = null;
                size = null;
                length = null;
            }

            public string FormatSpec()
            {
                var s = "";
                if (color != null)
                {
                    s += $"{color}, ";
                }

                if (type != null)
                {
                    s += $"{type}, ";
                }

                if (size != null)
                {
                    s += $"Ø{size}”, ";
                }

                if (length != null)
                {
                    s += $"L={length}’, ";
                }

                // remove last ,
                s = s.Substring(0, s.Length - 2);

                return s;
            }
        }

        #endregion

        [System.Serializable]
        public class TableRow
        {
            public int segment;
            public string spec;
        }

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