using System;
using System.Collections.Generic;
using Fusion;
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

        #region Adaptation from the old implementation

        // original scenario
        public VRC2.Scenarios.Scenario oldScenario;

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
                
                // TODO: add parameter to pass the old implementation to support before incident callback
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
        // private ScenariosManager _scenariosManager;
        //
        // [HideInInspector]
        // public ScenariosManager scenariosManager
        // {
        //     get
        //     {
        //         if (_scenariosManager == null)
        //         {
        //             _scenariosManager = FindFirstObjectByType<ScenariosManager>();
        //         }
        //
        //         return _scenariosManager;
        //     }
        // }
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


        private void OnGUI()
        {

            if (GUI.Button(new Rect(200, 10, 150, 50), "Start"))
            {
                var t = Helper.SecondNow();
                StartScenario(t);
            }
        }

        #endregion
    }
}