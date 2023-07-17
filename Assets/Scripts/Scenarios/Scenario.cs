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

        private bool started = false;
        private bool finished = false;

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
            print($"{this.GetType().Name}.Execute()");
            startTimestamp = timestamp;
            ready = true;
            started = false;
            finished = false;
        }

        private void Update()
        {
            if (!ready) return;

            if (finished) return;

            var sec = Helper.SecondNow();

            var localts = sec - startTimestamp;

            if (localts >= startInSec)
            {
                if (!started)
                {
                    print($"{this.GetType().Name} start @ {localts}");
                    // start it
                    started = true;
                    StartIncidents();
                    if (OnStart != null)
                    {
                        OnStart();
                    }
                }
                else
                {
                    // check whether it needs to stop this scenario
                    if (localts >= endInSec)
                    {
                        print($"{this.GetType().Name} finsh @ {localts}");
                        // time to stop it
                        finished = true;
                        ForceQuitIncidents();
                        if (OnFinish != null)
                        {
                            OnFinish();
                        }
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

            _name = scenario.name;

            // init _rawtime
            _rawTime = $"{scenario.start}{Helper.timeSep}{scenario.end}";
            // parse time in incidents
            Helper.ParseTime(_rawTime, ref startInSec, ref endInSec);
            // add incidents
            foreach (var icd in scenario.incidents)
            {
                Incident incident = gameObject.AddComponent<Incident>();
                incident.InitIncident(_name, icd.id, icd.time, icd.incident, icd.warning);
                AddIncident(incident);
            }
        }
    }
}