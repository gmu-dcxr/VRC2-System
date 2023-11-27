using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Base;
using VRC2.ScenariosV2.Tool;
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

        [HideInInspector]public int _id = 0;

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

        [ReadOnly]public List<Incident> parsedIncidents;

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

            // this.ScenarioStart += OnScenarioStart;
            // this.ScenarioFinish += OnScenarioFinish;
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
            if (sec < startInSec) return;

            // exceed time 
            if (endInSec > 0 && sec > endInSec) return;

            if (timeIncidentIdxMap.ContainsKey(sec) && !startedIncidents.Contains(sec))
            {
                // idx
                var idx = timeIncidentIdxMap[sec];
                // get incident
                var pi = parsedIncidents[idx];

                // run
                pi.RunIncident();

                startedIncidents.Add(sec);
            }

        }

        #endregion

        // #region Adaptation from the old version
        //
        // // original scenario
        // public VRC2.Scenarios.Scenario oldScenario;
        //
        // private int _taskStart
        // {
        //     get => oldScenario.taskStart;
        // }
        //
        // private int _taskEnd
        // {
        //     get => oldScenario.taskEnd;
        // }
        //
        // public System.Action ScenarioStart;
        // public System.Action ScenarioFinish;
        //
        // // SAGAT survey UI
        // private GameObject SAGATRoot;
        //
        // private SurveyController _surveyController;
        //
        // // warning controller
        // private WarningController _warningController;
        //
        // public WarningController warningController
        // {
        //     get
        //     {
        //         if (_warningController == null)
        //         {
        //             _warningController = GameObject.FindFirstObjectByType<WarningController>();
        //         }
        //
        //         return _warningController;
        //     }
        // }
        //
        // [HideInInspector]
        // public SurveyController surveyController
        // {
        //     get
        //     {
        //         if (SAGATRoot == null)
        //         {
        //             SAGATRoot = GameObject.FindWithTag(GlobalConstants.SAGATTag);
        //             _surveyController = SAGATRoot.GetComponent<SurveyController>();
        //         }
        //
        //         return _surveyController;
        //     }
        // }
        //
        private ScenariosManager _scenariosManager;

        [HideInInspector]
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
        //
        // [HideInInspector]
        // public bool warningShowing
        // {
        //     get { return warningController.showing; }
        // }
        //
        // #region Player
        //
        // private NetworkRunner _networkRunner;
        // private GameObject _localPlayer;
        // private GameObject _remotePlayer;
        //
        // [HideInInspector]
        // public NetworkRunner networkRunner
        // {
        //     get
        //     {
        //         if (_networkRunner == null)
        //         {
        //             _networkRunner = GameObject.FindFirstObjectByType<NetworkRunner>();
        //         }
        //
        //         return _networkRunner;
        //     }
        // }
        //
        // [HideInInspector]
        // public GameObject localPlayer // HighFidelityFirstPerson
        // {
        //     get
        //     {
        //         if (_localPlayer == null)
        //         {
        //             var players = GameObject.FindObjectsOfType<OVRCustomSkeleton>(includeInactive: true);
        //
        //             if (networkRunner == null || !networkRunner.IsRunning)
        //             {
        //                 if (players != null)
        //                 {
        //                     _localPlayer = players[0].gameObject;
        //                     Debug.LogWarning($"Find local player: {_localPlayer.name}");
        //                 }
        //             }
        //             else
        //             {
        //                 // find the object having input authority
        //                 foreach (var player in players)
        //                 {
        //                     var no = player.gameObject.GetComponent<NetworkObject>();
        //                     if (no.HasInputAuthority)
        //                     {
        //                         _localPlayer = no.gameObject;
        //
        //                         Debug.LogWarning($"Find local player: {_localPlayer.name}");
        //                     }
        //                     else
        //                     {
        //                         _remotePlayer = no.gameObject;
        //                         Debug.LogWarning($"Find remote player: {_localPlayer.name}");
        //                     }
        //                 }
        //             }
        //
        //         }
        //
        //         return _localPlayer;
        //     }
        // }
        //
        // [HideInInspector]
        // public GameObject remotePlayer // HighFidelityFirstPerson without 
        // {
        //     get
        //     {
        //         // get local player first, and the remote player will be set
        //         var lp = localPlayer;
        //         return _remotePlayer;
        //     }
        // }
        //
        //
        //
        // #endregion
        //
        // public virtual void OnScenarioStart()
        // {
        //     UpdateInstruction();
        // }
        //
        //
        //
        // public virtual void OnScenarioFinish()
        // {
        //
        // }
        //
        // public virtual void UpdateInstruction()
        // {
        //     if (_taskStart > 0 && _taskEnd > 0)
        //     {
        //         scenariosManager.UpdateInstruction(_taskStart, _taskEnd);
        //     }
        // }
        //
        // public virtual void OnIncidentFinish(int obj)
        // {
        //     // hide warning
        //     HideWarning();
        //
        //     // var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);
        //     // print($"{_name} #{obj} {name}");
        //     // Invoke(name, 0);
        // }
        //
        // public virtual void OnIncidentStart(int obj, float? delay)
        // {
        //     // show warning
        //     
        //     // TODO: start
        //     var msg = GetRightMessage(obj, scenariosManager.condition.Context, scenariosManager.condition.Amount);
        //     ShowWarning(_name, obj, msg, delay);
        //     //// TODO: end
        //     
        //     // var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);
        //     // print($"{_name} #{obj} {name}");
        //     // Invoke(name, 0);
        // }
        //
        // public void ShowSAGAT()
        // {
        //     surveyController.Show();
        // }
        //
        // public void HideSAGAT()
        // {
        //     surveyController.Hide();
        // }
        //
        // public void ShowWarning(string sname, int idx, string msg, float? delay)
        // {
        //     if (msg == "") return;
        //
        //     print($"Show warning: {msg}");
        //     warningController.Show("Warning", sname, idx, msg, delay);
        // }
        //
        // public void HideWarning()
        // {
        //     print("Hide warning");
        //     warningController.Hide(true);
        // }
        //
        //
        // #endregion

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

            if (GUI.Button(new Rect(200, 10, 150, 50), "Start"))
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
    }
}