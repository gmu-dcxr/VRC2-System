using System;
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

        // (time, incident index in `parsedIncidents`)
        public Dictionary<int, int> timeIncidentIdxMap;
        public HashSet<int> startedIncidents; // started incidents

        #endregion

        #endregion

        #region Incident Control

        // the offset
        private int startTimestamp { get; set; }

        // start / end in second
        [HideInInspector] public int startInSec;
        [HideInInspector] public int endInSec;
        [HideInInspector] public bool started = false;


        #endregion

        #region Vehicles

        private Vehicle.Crane _crane;
        private Vehicle.Drone _drone;

        public Vehicle.Crane crane
        {
            get
            {
                if (_crane == null)
                {
                    _crane = FindObjectOfType<Vehicle.Crane>();
                }

                return _crane;
            }
        }

        public Vehicle.Drone drone
        {
            get
            {
                if (_drone == null)
                {
                    _drone = FindObjectOfType<Vehicle.Drone>();
                }

                return _drone;
            }
        }



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

            var cname = arr[0];
            var idx = int.Parse(arr[2]);
            var normal = IsNormal(refer);

            // crane
            if (cname.Equals(crane.ClsName))
            {
                return crane.GetIncident(idx, normal);
            }

            // drone
            if (cname.Equals(drone.ClsName))
            {
                return drone.GetIncident(idx, normal);
            }

            // default
            return null;
        }

        private void ParseIncidents()
        {
            if (_incidents == null) return;
            parsedIncidents = new List<Incident>();

            timeIncidentIdxMap = new Dictionary<int, int>();

            var count = _incidents.Count;
            for (var i = 0; i < count; i++)
            {
                var inci = GetIncident(_incidents[i]);
                // parse time
                var t = ParseTime(_incidents[i].time);

                parsedIncidents.Add(inci);

                // add map
                timeIncidentIdxMap.Add(t, i);
            }
        }

        // parse time in scenario definition file
        private int ParseTime(string str)
        {
            var s = 0;
            var e = 0;
            Helper.ParseTime(str, ref s, ref e);
            return s;
        }

        #region Scenario Control

        public void Start()
        {
            ParseYamlFile();
        }

        public void StartScenario(int ts)
        {
            startTimestamp = ts;
            var c = parsedIncidents.Count;
            for (var i = 0; i < c; i++)
            {
                parsedIncidents[i].RunIncident();
            }

            if (startedIncidents == null)
            {
                startedIncidents = new HashSet<int>();
            }
            else
            {
                startedIncidents.Clear();
            }
        }

        public void FixedUpdate()
        {
            if (!started) return;

            // current second
            var sec = Helper.SecondNow() - startTimestamp;

            // not time to start
            if (sec < startInSec) return;

            // exceed time 
            if (endInSec > 0 && sec > endInSec) return;

            if (timeIncidentIdxMap.ContainsKey(sec) && !startedIncidents.Contains(sec))
            {
                // idx
                var idx = timeIncidentIdxMap[sec];
                // get incident
                var pi = parsedIncidents[idx];


                print($"start incident: {pi.callback}");

                startedIncidents.Add(sec);
            }

        }

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var t = Helper.SecondNow();
                StartScenario(t);
            }
        }

        #endregion
    }
}