using System;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Unity.VisualScripting;
using YamlDotNet.Serialization;

namespace VRC2.Scenarios
{
    public class Scenario : MonoBehaviour
    {
        public List<Incident> incidents = null;

        private int startInSec;
        private int endInSec;

        private string _rawTime;

        private string _name;
        private string _shortName;

        private int startTimestamp { get; set; }

        private bool started;

        public System.Action OnStart;
        public System.Action OnFinish;

        private bool ready = false;

        [HideInInspector] public YamlHelper.Scenario scenario;

        public void AddIncident(Incident incident)
        {
            if (incidents == null)
            {
                incidents = new List<Incident>();
            }
            incidents.Add(incident);
        }

        public void Execute(int timestamp)
        {
            startTimestamp = timestamp;
            ready = true;
        }

        private void Update()
        {
            if (!ready) return;

            var sec = Helper.SecondNow();

            if (sec - startTimestamp >= startInSec)
            {
                if (!started)
                {
                    // start it
                    started = true;
                    StartIncidents();
                    OnStart();
                }
                else
                {
                    // check whether it needs to stop this scenario
                    if (sec - startTimestamp >= endInSec)
                    {
                        // time to stop it
                        started = false;
                        ForceQuitIncidents();
                        OnFinish();
                    }
                }
            }
        }

        void StartIncidents()
        {
            foreach (var incident in incidents)
            {
                incident.Execute(startTimestamp);
            }
        }

        void ForceQuitIncidents()
        {
            foreach (var incident in incidents)
            {
                incident.ForceQuit();
            }
        }

        public void InitFromFile(string filename)
        {
            var path = Helper.GetConfigureFile(Application.dataPath, filename);
            var text = System.IO.File.ReadAllText(path);
            var deser = new DeserializerBuilder().Build();
            scenario = deser.Deserialize<YamlHelper.Scenario>(text);
            
            // init _rawtime
            _rawTime = $"{scenario.start}{Helper.timeSep}{scenario.end}";
            // parse time in incidents
            Helper.ParseTime(_rawTime, ref startInSec, ref endInSec);
            // add incidents
            foreach (var icd in scenario.incidents)
            {
                Incident incident = gameObject.AddComponent<Incident>();
                incident.InitIncident(icd.incident, icd.warning, icd.time);
                AddIncident(incident);
            }
        }
    }
}