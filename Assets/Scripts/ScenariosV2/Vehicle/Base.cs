using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Helper = VRC2.Scenarios.Helper;
using VRC2.ScenariosV2.Base;
using YamlDotNet.Serialization;
using Incident = VRC2.ScenariosV2.Base.Incident;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Base : MonoBehaviour
    {
        #region Attributes

        private string name;
        private string desc;
        private List<string> variables;
        private string @gameObject;
        private YamlParser.Incidents incidents;

        #endregion

        #region Parsed Attributes

        private List<Incident> normals;
        private List<Incident> accidents;

        #endregion

        #region Methods

        public void ParseYamlFile(string name)
        {
            var path = Helper.GetConfigureFile(Application.dataPath, name);
            print(path);

            var text = System.IO.File.ReadAllText(path);
            print(text);

            var deser = new DeserializerBuilder().Build();
            var v = deser.Deserialize<YamlParser.Vehicle>(text);

            ParseYamlVehicle(v);
        }

        public void ParseYamlVehicle(YamlParser.Vehicle v)
        {
            this.name = v.name;
            this.desc = v.desc;
            this.variables = v.variables;

            // parse normals
            if (v.incidents.normals != null)
            {
                this.normals = new List<Incident>();
                var count = v.incidents.normals.Count;
                for (var i = 0; i < count; i++)
                {
                    var inci = v.incidents.normals[i];
                    Incident incident = new Incident();
                    incident.ParseYamlIncident(inci);
                    this.normals.Add(incident);
                }
            }

            // parse accidents
            if (v.incidents.accidents != null)
            {
                this.accidents = new List<Incident>();
                var count = v.incidents.accidents.Count;
                for (var i = 0; i < count; i++)
                {
                    var inci = v.incidents.accidents[i];
                    Incident incident = new Incident();
                    incident.ParseYamlIncident(inci);
                    this.accidents.Add(incident);
                }
            }
        }



        #endregion

        #region Helper functions

        public Incident GetIncident(int idx, bool normal)
        {
            var li = this.normals;
            if (!normal)
            {
                li = this.accidents;
            }

            if (li == null) return null;

            var count = li.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = li[i];
                if (inci.id == idx)
                {
                    return inci;
                }
            }

            Debug.LogError($"Can not find Incident#{idx}");
            return null;
        }

        private List<Incident> GetTxIncidents(string tx, bool normal)
        {
            if (!this.variables.Contains(tx))
            {
                Debug.LogError($"{tx} is not in {this.variables}.");
                return null;
            }

            var li = this.normals;
            if (!normal)
            {
                li = this.accidents;
            }

            if (li == null) return null;

            List<Incident> result = new List<Incident>();

            var count = li.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = li[i];
                if (inci.time.Contains(tx))
                {
                    result.Add(inci);
                }
            }

            return result;
        }

        public List<Incident> GetNormalIncident(string tx)
        {
            return GetTxIncidents(tx, true);
        }

        public List<Incident> GetAccidentIncident(string tx)
        {
            return GetTxIncidents(tx, false);
        }

        public Incident GetNormalIncident(int idx)
        {
            return GetIncident(idx, true);
        }

        public Incident GetAccidentIncident(int idx)
        {
            return GetIncident(idx, false);
        }

        #endregion
    }
}