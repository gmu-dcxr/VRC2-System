using System;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Unity.VisualScripting;
using YamlDotNet.Serialization;

namespace VRC2.Scenarios
{
    public enum ScenarioCallback
    {
        Start = 1,
        Finish = 2,
    }

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

        public System.Action ScenarioStart;
        public System.Action ScenarioFinish;

        public System.Action<int> IncidentStart;
        public System.Action<int> IncidentFinish;

        private string _clsName = "";

        public string ClsName
        {
            get => _clsName;
        }

        private bool ready = false;

        [HideInInspector] public YamlHelper.Scenario scenario;

        public void Start()
        {
            _clsName = this.GetType().Name;
        }

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
            print($"{ClsName}.Execute()");
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
                    print($"Scenario {_name} Start @ {localts}");
                    // start it
                    started = true;
                    StartIncidents();
                    if (ScenarioStart != null)
                    {
                        ScenarioStart();
                    }
                }
                else
                {
                    // check whether it needs to stop this scenario
                    if (localts >= endInSec)
                    {
                        print($"Scenario {_name} Finish @ {localts}");
                        // time to stop it
                        finished = true;
                        ForceQuitIncidents();
                        if (ScenarioFinish != null)
                        {
                            ScenarioFinish();
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
                // bind actions
                incident.OnStart += OnIncidentStart;
                incident.OnFinish += OnIncidentFinish;
                AddIncident(incident);
            }
        }

        private void OnIncidentFinish(int obj)
        {
            print($"{_name} #{obj} OnIncidentFinish");
            if (IncidentFinish != null)
            {
                IncidentFinish(obj);
            }
        }

        private void OnIncidentStart(int obj)
        {
            print($"{_name} #{obj} OnIncidentStart");
            if (IncidentStart != null)
            {
                IncidentStart(obj);
            }
        }

        public void CheckIncidentsCallbacks()
        {
            bool pass = true;

            var @namespace = "VRC2.Scenarios.ScenarioFactory";
            var myClassType = Type.GetType($"{@namespace}.{ClsName}");

            foreach (var incident in incidents)
            {
                var _id = incident.ID;
                var name1 = Helper.GetIncidentCallbackName(ClsName, _id, ScenarioCallback.Start);
                var name2 = Helper.GetIncidentCallbackName(ClsName, _id, ScenarioCallback.Finish);

                if (myClassType.GetMethod(name1) == null)
                {
                    pass = false;
                    Debug.LogError($"[{ClsName}] missing method: {name1}");
                }

                if (myClassType.GetMethod(name2) == null)
                {
                    pass = false;
                    Debug.LogError($"[{ClsName}] missing method: {name2}");
                }
            }

            Debug.LogWarning($"{ClsName} Check Incidents Callbacks Result: {pass}");
        }
    }
}