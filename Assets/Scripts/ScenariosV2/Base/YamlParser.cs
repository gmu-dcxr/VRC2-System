using System.Collections.Generic;
using JetBrains.Annotations;

namespace VRC2.ScenariosV2.Base
{
    public class YamlParser
    {
        public class Incident
        {
            public int id { get; set; }
            public string condition { get; set; }
            public string time { get; set; }
            [CanBeNull] public string type { get; set; }
            public string desc { get; set; }
            [CanBeNull] public string warning { get; set; }
        }

        public class Incidents
        {
            [CanBeNull] public List<Incident> normals { get; set; }
            [CanBeNull] public List<Incident> accidents { get; set; }
        }

        public class Vehicle
        {
            public string name { get; set; }
            public string desc { get; set; }
            public List<string> variables { get; set; }
            public string gameObject { get; set; }
            public Incidents incidents { get; set; }
        }
    }
}