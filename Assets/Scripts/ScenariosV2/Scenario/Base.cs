using System.Collections.Generic;
using UnityEngine;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Base;
using YamlDotNet.Serialization;
using Incident = VRC2.ScenariosV2.Base.Incident;

namespace VRC2.ScenariosV2.Scenario
{
    public class Base : MonoBehaviour
    {
        #region Attributes

        #region Raw from yaml file

        private string _name;
        private string _desc;
        private string _normal;
        private string _accident;
        private List<YamlParser.Refer> _incidents;

        #endregion

        #region Parse Incidents

        public string ClsName
        {
            get => GetType().Name;
        }

        public string DefaultYamlFile
        {
            get => $"{ClsName}.yml";
        }

        public List<Incident> parsedIncidents;

        #endregion

        #endregion

        public void ParseYamlFile()
        {
            ParseYamlFile(DefaultYamlFile);
        }

        public void ParseYamlFile(string name)
        {
            var path = Helper.GetConfigureFile(Application.dataPath, name);
            print(path);

            var text = System.IO.File.ReadAllText(path);
            print(text);

            var deser = new DeserializerBuilder().Build();
            var s = deser.Deserialize<YamlParser.Scenario>(text);

            ParseYamlScenario(s);

            ParseIncidents();
        }

        public void ParseYamlScenario(YamlParser.Scenario s)
        {
            _name = s.name;
            _desc = s.desc;
            _normal = s.normal;
            _accident = s.accident;
            _incidents = s.incidents;
        }

        public bool IsNormal(YamlParser.Refer r)
        {
            return r.refer[1].ToLower().Equals("normals");
        }

        private Incident GetIncident(YamlParser.Refer refer)
        {
            var arr = refer.refer;
            if (arr[0] == "Crane")
            {
                var cr = FindObjectOfType<Vehicle.Crane>();
                return cr.GetIncident(int.Parse(arr[2]), IsNormal(refer));
            }

            return null;
        }

        private void ParseIncidents()
        {
            if (_incidents == null) return;
            parsedIncidents = new List<Incident>();

            var count = _incidents.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = GetIncident(_incidents[i]);
                parsedIncidents.Add(inci);
            }
        }
    }
}