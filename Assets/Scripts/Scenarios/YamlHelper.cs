using System.Collections.Generic;

namespace VRC2.Scenarios
{
    public class YamlHelper
    {
        public class Incident
        {
            public int id { get; set; }
            public string time { get; set; }
            public string incident { get; set; }
            public string warning { get; set; }
        }

        public class WarningVariant
        {
            public int id { get; set; }
            public string warning { get; set; }
        }

        public class Scenario
        {
            public string name { get; set; }
            public string desc { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public int taskStart { get; set; }
            public int taskEnd { get; set; }
            public List<Incident> incidents { get; set; }
            public List<WarningVariant> context { get; set; }
            public List<WarningVariant> amount { get; set; }
        }
    }
}