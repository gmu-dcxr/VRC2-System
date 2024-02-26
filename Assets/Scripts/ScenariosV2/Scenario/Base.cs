using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Network;
using VRC2.Utility;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Base;
using VRC2.ScenariosV2.Tool;
using YamlDotNet.Serialization;
using Incident = VRC2.ScenariosV2.Base.Incident;

using TaskBase = VRC2.Task.Base;

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
        private string startTimeRaw;
        private string endTimeRaw;

        private int taskStart;
        private int taskEnd;
        [ReadOnly] public string task; // task config filename

        private List<YamlParser.Refer> _incidents;

        #endregion

        #region Parse Incidents

        [HideInInspector] public int _id = 0;

        public int ID
        {
            get => _id;
        }

        public string ClsName
        {
            get => GetType().Name;
        }

        public string DefaultYamlFile
        {
            get => $"{ClsName}.yml";
        }

        [ReadOnly] public List<Incident> parsedIncidents;

        // (time, incident index in `parsedIncidents`)
        public Dictionary<int, List<int>> timeIncidentIdxMap;
        public HashSet<int> startedIncidents; // started incidents

        [ReadOnly] public TaskBase taskBase;

        #endregion

        #endregion

        #region Incident Control

        // the offset
        private int startTimestamp { get; set; }

        // start / end in second
        [HideInInspector] public int startInSec;
        [HideInInspector] public int endInSec;
        [HideInInspector] public bool started = false;

        private bool scenarioStarted = false;
        private bool scenarioFinished = false;

        // scenario start/finish events
        public System.Action ScenarioStart;
        public System.Action ScenarioFinish;

        #endregion

        #region Vehicles

        private Vehicle.Crane _crane;
        private Vehicle.Drone _drone;
        private Vehicle.Truck _truck;
        private Vehicle.CraneTruck _craneTruck;
        private Vehicle.Forklift _forklift;

        // adapation for Scenario 4
        private Vehicle.Irrelevant _irrelevant;

        // Scenario 6
        private Vehicle.Electrocutions _electrocutions;

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

        public Vehicle.Truck truck
        {
            get
            {
                if (_truck == null)
                {
                    _truck = FindObjectOfType<Vehicle.Truck>();
                }

                return _truck;
            }
        }

        public Vehicle.CraneTruck craneTruck
        {
            get
            {
                if (_craneTruck == null)
                {
                    _craneTruck = FindObjectOfType<Vehicle.CraneTruck>();
                }

                return _craneTruck;
            }
        }

        public Vehicle.Forklift forklift
        {
            get
            {
                if (_forklift == null)
                {
                    _forklift = FindObjectOfType<Vehicle.Forklift>();
                }

                return _forklift;
            }
        }

        public Vehicle.Irrelevant irrelevant
        {
            get
            {
                if (_irrelevant == null)
                {
                    _irrelevant = FindObjectOfType<Vehicle.Irrelevant>();
                }

                return _irrelevant;
            }
        }

        public Vehicle.Electrocutions electrocutions
        {
            get
            {
                if (_electrocutions == null)
                {
                    _electrocutions = FindObjectOfType<Vehicle.Electrocutions>();
                }

                return _electrocutions;
            }
        }

        private ScenariosManager _scenariosManager;

        public ScenariosManager scenariosManager
        {
            get
            {
                if (_scenariosManager == null)
                {
                    _scenariosManager = FindFirstObjectByType<ScenariosManager>();
                }

                return _scenariosManager;
            }
        }

        private WarningController _warningController;

        public WarningController warningController
        {
            get
            {
                if (_warningController == null)
                {
                    _warningController = FindFirstObjectByType<WarningController>();
                }

                return _warningController;
            }
        }

        private P1P2RoleChecker _roleChecker;

        public P1P2RoleChecker roleChecker
        {
            get
            {
                if (_roleChecker == null)
                {
                    _roleChecker = FindFirstObjectByType<P1P2RoleChecker>();
                }

                return _roleChecker;
            }
        }

        // change order
        private List<int> changeOrderIndices = new List<int>();

        #endregion

        [Header("Debug UI")] public bool showDebugUI = true;

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
            startTimeRaw = s.start;
            endTimeRaw = s.end;
            taskStart = s.taskStart;
            taskEnd = s.taskEnd;
            task = s.task;
            _incidents = s.incidents;

            // parse time
            var rawTime = $"{startTimeRaw}{Helper.timeSep}{endTimeRaw}";
            // parse time in incidents
            Helper.ParseTime(rawTime, ref startInSec, ref endInSec);
        }

        public bool IsNormal(YamlParser.Refer r)
        {
            return r.refer[1].ToLower().Equals("normals");
        }

        private (Incident, bool) GetIncident(YamlParser.Refer refer)
        {
            var arr = refer.refer;

            var cname = arr[0];
            var idx = int.Parse(arr[2]);
            var normal = IsNormal(refer);
            var warning = refer.warning;
            var changeOrder = refer.changeOrder;

            Incident res = null;

            // crane
            if (cname.Equals(crane.ClsName))
            {
                res = crane.GetIncident(idx, normal);
            }

            // drone
            if (cname.Equals(drone.ClsName))
            {
                res = drone.GetIncident(idx, normal);
            }

            // truck
            if (cname.Equals(truck.ClsName))
            {
                res = truck.GetIncident(idx, normal);
            }

            // cranetruck
            if (cname.Equals(craneTruck.ClsName))
            {
                res = craneTruck.GetIncident(idx, normal);
            }

            // forklift
            if (cname.Equals(forklift.ClsName))
            {
                res = forklift.GetIncident(idx, normal);
            }

            // irrelevant
            if (cname.Equals(irrelevant.ClsName))
            {
                res = irrelevant.GetAccidentIncident(idx);
            }

            // electrocutions
            if (cname.Equals(electrocutions.ClsName))
            {
                res = electrocutions.GetAccidentIncident(idx);
            }

            if (res != null)
            {
                // update reference warning
                res.showWarning = warning;
            }

            // return change order
            return (res, changeOrder);
        }

        private void ParseIncidents()
        {
            if (_incidents == null) return;
            parsedIncidents = new List<Incident>();

            timeIncidentIdxMap = new Dictionary<int, List<int>>();

            // clear
            changeOrderIndices.Clear();

            var count = _incidents.Count;
            for (var i = 0; i < count; i++)
            {
                var (inci, changeOrder) = GetIncident(_incidents[i]);
                // parse time
                var t = ParseTime(_incidents[i].time);

                if (!timeIncidentIdxMap.ContainsKey(t))
                {
                    var indices = new List<int>();
                    indices.Add(i);
                    timeIncidentIdxMap.Add(t, indices);
                }
                else
                {
                    var indices = timeIncidentIdxMap[t];
                    indices.Add(i);
                    timeIncidentIdxMap[t] = indices;
                }

                parsedIncidents.Add(inci);

                if (changeOrder)
                {
                    // add index
                    changeOrderIndices.Add(i);
                }
            }

            print($"Change Order Index: {string.Join(",", changeOrderIndices)}");
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

            // load task
            LoadTask();

            this.ScenarioStart += OnScenarioStart;
            this.ScenarioFinish += OnScenarioFinish;
        }

        public void StartScenario(int ts)
        {
            if (startedIncidents == null)
            {
                startedIncidents = new HashSet<int>();
            }
            else
            {
                startedIncidents.Clear();
            }

            startTimestamp = ts;
            started = true;
            print($"Set Scenario {name} to start @ {startTimestamp}");
        }

        public void Execute(int timestamp)
        {
            StartScenario(timestamp);
        }

        public void OverrideStartEnd(int start, int end)
        {
            startInSec = start;
            endInSec = end;
            print($"OverrideStartEnd for {name}: {startInSec} - {endInSec}");
        }

        public void FixedUpdate()
        {
            if (!started) return;

            // current second
            var sec = Helper.SecondNow() - startTimestamp;

            // not time to start
            if (sec < startInSec)
            {
                return;
            }
            else
            {
                if (!scenarioStarted)
                {
                    // scenario started
                    print($"Scenario {_name} Start @ {sec}");
                    // update flag first
                    scenarioStarted = true;

                    if (ScenarioStart != null)
                    {
                        ScenarioStart();
                    }
                }
            }

            // exceed time 
            if (endInSec > 0 && sec > endInSec)
            {
                if (!scenarioFinished)
                {
                    // scenario ended
                    print($"Scenario {_name} Finish @ {sec}");
                    // update flag first
                    scenarioFinished = true;

                    if (ScenarioFinish != null)
                    {
                        ScenarioFinish();
                    }
                }

                return;
            }

            if (timeIncidentIdxMap.ContainsKey(sec) && !startedIncidents.Contains(sec))
            {
                // idx
                var indices = timeIncidentIdxMap[sec];

                foreach (var idx in indices)
                {
                    // change order
                    if (changeOrderIndices.Contains(idx))
                    {
                        // trigger change order
                        // TODO: maybe better to add a timer?
                        StartCoroutine(ChangeOrder());
                    }

                    StartCoroutine(CoroutineStartIncident(idx));
                }

                // exclude it
                startedIncidents.Add(sec);
            }
        }

        IEnumerator CoroutineStartIncident(int idx)
        {
            // get incident
            var pi = parsedIncidents[idx];
            pi.RunIncident();
            yield break;
        }

        #endregion

        #region Debug

        void StartIncident(int idx)
        {
            if (idx > parsedIncidents.Count)
            {
                print("Out of boundary of StartIncident.");
                return;
            }

            var pi = parsedIncidents[idx - 1];
            pi.RunIncident();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(200, 10, 150, 50), $"Start {name}"))
            {
                var t = Helper.SecondNow();
                StartScenario(t);
            }

            if (!showDebugUI) return;

            // only enable for debugging when scenario manager doesn't set scenarios and runner is not running
            var runner = GameObject.FindObjectOfType<NetworkRunner>();
            if (runner != null && runner.IsRunning && scenariosManager.scenarios != null &&
                scenariosManager.scenarios.Count > 0) return;

            if (GUI.Button(new Rect(10, 10, 150, 30), $"Start {ClsName}"))
            {
                var t = Helper.SecondNow();
                StartScenario(t);
            }

            if (GUI.Button(new Rect(10, 50, 150, 30), $"Start {1}"))
            {
                StartIncident(1);
            }

            if (GUI.Button(new Rect(10, 100, 150, 30), $"Start {2}"))
            {
                StartIncident(2);
            }

            if (GUI.Button(new Rect(10, 150, 150, 30), $"Start {3}"))
            {
                StartIncident(3);
            }

            if (GUI.Button(new Rect(10, 200, 150, 30), $"Start {4}"))
            {
                StartIncident(4);
            }

            if (GUI.Button(new Rect(10, 250, 150, 30), $"Start {5}"))
            {
                StartIncident(5);
            }

            if (GUI.Button(new Rect(10, 300, 150, 30), $"Start {6}"))
            {
                StartIncident(6);
            }

            if (GUI.Button(new Rect(10, 350, 150, 30), $"Start {7}"))
            {
                StartIncident(7);
            }

            if (GUI.Button(new Rect(10, 400, 150, 30), $"Start {8}"))
            {
                StartIncident(8);
            }

            if (GUI.Button(new Rect(10, 450, 150, 30), $"Start {9}"))
            {
                StartIncident(9);
            }

            if (GUI.Button(new Rect(10, 500, 150, 30), $"Start {10}"))
            {
                StartIncident(10);
            }
        }

        #endregion

        public virtual void OnScenarioStart()
        {
            print($"Invoke {name} OnScenarioStart");
            UpdateInstruction();
        }

        public virtual void OnScenarioFinish()
        {
            print($"Invoke {name} OnScenarioFinish");
        }

        #region Task Instruction

        public virtual void UpdateInstruction()
        {
            // table instruction

            if (taskBase != null)
            {
                taskBase.UpdateTableInstruction(roleChecker.IsP1());
            }

            // wall instruction
            if (taskStart > 0 && taskEnd > 0)
            {
                scenariosManager.UpdateInstruction(taskStart, taskEnd);
            }
        }

        /// <summary>
        /// Load task that is defined in the scenario config file by the field `task`
        /// </summary>
        public virtual void LoadTask()
        {
            var name = task.Split('.')[0];
            // find task under VRC2.Task
            var clsName = $"VRC2.Task.{name}";
            print($"Load Task: {clsName}");
            var myClassType = Type.GetType(clsName);

            taskBase = (TaskBase)FindObjectOfType(myClassType);
        }

        /// <summary>
        /// Trigger change order
        /// </summary>
        public virtual IEnumerator ChangeOrder()
        {
            print("Invoke ChangeOrder");
            if (taskBase != null)
            {
                Debug.LogWarning($"Change order for {ClsName}");
                taskBase.ChangeOrder(roleChecker.IsP1());
            }

            yield break;
        }

        #endregion
    }
}