using System;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Unity.VisualScripting;
using VRC2.Conditions;
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

        [Header("Debug UI")] public bool showDebugUI = true;
        
        [HideInInspector] public List<Incident> incidents = null;

        [HideInInspector] public int startInSec;
        [HideInInspector] public int endInSec;

        private string _rawTime;

        private int _id = -1;
        private string _name;
        private string _shortName;
        private int _taskStart;
        private int _taskEnd;

        private List<YamlHelper.WarningVariant> _context;
        private List<YamlHelper.WarningVariant> _amount;

        private int startTimestamp { get; set; }

        private bool started = false;
        private bool finished = false;

        public System.Action ScenarioStart;
        public System.Action ScenarioFinish;

        // SAGAT survey UI
        private GameObject SAGATRoot;

        private SurveyController _surveyController;

        // warning controller
        private WarningController _warningController;

        public WarningController warningController
        {
            get
            {
                if (_warningController == null)
                {
                    _warningController = GameObject.FindFirstObjectByType<WarningController>();
                }

                return _warningController;
            }
        }

        [HideInInspector]
        public SurveyController surveyController
        {
            get
            {
                if (SAGATRoot == null)
                {
                    SAGATRoot = GameObject.FindWithTag(GlobalConstants.SAGATTag);
                    _surveyController = SAGATRoot.GetComponent<SurveyController>();
                }

                return _surveyController;
            }
        }

        public string ClsName
        {
            get => GetType().Name;
        }

        public int ID
        {
            get
            {
                if (_id < 0)
                {
                    _id = 0;
                    var ch = ClsName[ClsName.Length - 1];
                    try
                    {
                        _id = int.Parse($"{ch}");
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Failed to parse ID for {ClsName}");
                    }
                }

                return _id;
            }
        }

        public string name
        {
            get => _name;

            set => _name = value;
        }

        private bool ready = false;

        [HideInInspector] public YamlHelper.Scenario scenario;

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

        [HideInInspector]
        public bool warningShowing
        {
            get { return warningController.showing; }
        }

        #region Player

        private NetworkRunner _networkRunner;
        private GameObject _localPlayer;
        private GameObject _remotePlayer;

        [HideInInspector]
        public NetworkRunner networkRunner
        {
            get
            {
                if (_networkRunner == null)
                {
                    _networkRunner = GameObject.FindFirstObjectByType<NetworkRunner>();
                }

                return _networkRunner;
            }
        }

        [HideInInspector]
        public GameObject localPlayer // HighFidelityFirstPerson
        {
            get
            {
                if (_localPlayer == null)
                {
                    var players = GameObject.FindObjectsOfType<OVRCustomSkeleton>(includeInactive: true);

                    if (networkRunner == null || !networkRunner.IsRunning)
                    {
                        if (players != null)
                        {
                            _localPlayer = players[0].gameObject;
                            Debug.LogWarning($"Find local player: {_localPlayer.name}");
                        }
                    }
                    else
                    {
                        // find the object having input authority
                        foreach (var player in players)
                        {
                            var no = player.gameObject.GetComponent<NetworkObject>();
                            if (no.HasInputAuthority)
                            {
                                _localPlayer = no.gameObject;

                                Debug.LogWarning($"Find local player: {_localPlayer.name}");
                            }
                            else
                            {
                                _remotePlayer = no.gameObject;
                                Debug.LogWarning($"Find remote player: {_localPlayer.name}");
                            }
                        }
                    }

                }

                return _localPlayer;
            }
        }

        [HideInInspector]
        public GameObject remotePlayer // HighFidelityFirstPerson without 
        {
            get
            {
                // get local player first, and the remote player will be set
                var lp = localPlayer;
                return _remotePlayer;
            }
        }



        #endregion

        public virtual void OnGUI()
        {
            if (!showDebugUI) return;
            
            // only enable for debugging when scenario manager doesn't set scenarios and runner is not running
            var runner = GameObject.FindObjectOfType<NetworkRunner>();
            if (runner != null && runner.IsRunning && scenariosManager.scenarios != null &&
                scenariosManager.scenarios.Count > 0) return;

            if (GUI.Button(new Rect(10, 10, 150, 30), $"Start {ClsName}"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);
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

            if (GUI.Button(new Rect(10, 550, 150, 30), $"Normal"))
            {
                StartNormalIncident();
            }

            if (GUI.Button(new Rect(200, 50, 150, 30), $"End {1}"))
            {
                EndIncident(1);
            }

            if (GUI.Button(new Rect(200, 100, 150, 30), $"End {2}"))
            {
                EndIncident(2);
            }

            if (GUI.Button(new Rect(200, 150, 150, 30), $"End {3}"))
            {
                EndIncident(3);
            }

            if (GUI.Button(new Rect(200, 200, 150, 30), $"End {4}"))
            {
                EndIncident(4);
            }

            if (GUI.Button(new Rect(200, 250, 150, 30), $"End {5}"))
            {
                EndIncident(5);
            }

            if (GUI.Button(new Rect(200, 300, 150, 30), $"End {6}"))
            {
                EndIncident(6);
            }

            if (GUI.Button(new Rect(200, 350, 150, 30), $"End {7}"))
            {
                EndIncident(7);
            }

            if (GUI.Button(new Rect(200, 400, 150, 30), $"End {8}"))
            {
                EndIncident(8);
            }

            if (GUI.Button(new Rect(200, 450, 150, 30), $"End {9}"))
            {
                EndIncident(9);
            }

            if (GUI.Button(new Rect(200, 500, 150, 30), $"End {10}"))
            {
                EndIncident(10);
            }
        }

        public void Start()
        {
            InitFromFile();
            CheckIncidentsCallbacks();

            this.ScenarioStart += OnScenarioStart;
            this.ScenarioFinish += OnScenarioFinish;
        }

        public virtual void OnScenarioStart()
        {
            UpdateInstruction();
        }

        public virtual void OnScenarioFinish()
        {

        }

        // The configure file of each scenario is independent, so the start time always is 0.
        // When scenarios are sequenced, it's necessary to override them
        public void OverrideStartEnd(int start, int end)
        {
            startInSec = start;
            endInSec = end;
            print($"OverrideStartEnd for {name}: {startInSec} - {endInSec}");
        }

        public void AddIncident(Incident incident)
        {
            if (incidents == null)
            {
                incidents = new List<Incident>();
            }

            incidents.Add(incident);
        }

        public Incident GetIncident(int idx)
        {
            if (incidents == null || idx > incidents.Count) return null;

            foreach (var incident in incidents)
            {
                if (incident.ID == idx) return incident;
            }

            return null;
        }

        public void Execute(int timestamp)
        {
            startTimestamp = timestamp;
            ready = true;
            started = false;
            finished = false;
            // StartNormalIncident();
        }

        void FixedUpdate()
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
                    // it never ends when startInSec == endInSec. Useful for Background incidents
                    if (endInSec > startInSec && localts >= endInSec)
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

        public virtual void InitFromFile()
        {
            var filename = $"{ClsName}.yml";
            var path = Helper.GetConfigureFile(Application.dataPath, filename);
            var text = System.IO.File.ReadAllText(path);
            var deser = new DeserializerBuilder().Build();
            scenario = deser.Deserialize<YamlHelper.Scenario>(text);

            _name = scenario.name;
            _taskStart = scenario.taskStart;
            _taskEnd = scenario.taskEnd;

            if (scenario.context != null)
            {
                _context = scenario.context;
            }
            else
            {
                // to avoid null
                _context = new List<YamlHelper.WarningVariant>();
            }

            if (scenario.amount != null)
            {
                _amount = scenario.amount;
            }
            else
            {
                // to avoid null
                _amount = new List<YamlHelper.WarningVariant>();
            }

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

        public virtual void StartNormalIncident()
        {

        }

        public virtual void OnIncidentFinish(int obj)
        {
            // hide warning
            HideWarning();

            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);
            print($"{_name} #{obj} {name}");
            Invoke(name, 0);
        }

        public virtual void OnIncidentStart(int obj)
        {
            // show warning
            var msg = GetRightMessage(obj, scenariosManager.condition.Context, scenariosManager.condition.Amount);
            ShowWarning(_name, obj, msg);
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);
            print($"{_name} #{obj} {name}");
            Invoke(name, 0);
        }

        public virtual void CheckIncidentsCallbacks()
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

        public void ShowSAGAT()
        {
            surveyController.Show();
        }

        public void HideSAGAT()
        {
            surveyController.Hide();
        }

        public void ShowWarning(string sname, int idx, string msg)
        {
            if (msg == "") return;

            print($"Show warning: {msg}");
            warningController.Show("Warning", sname, idx, msg);
        }

        public void HideWarning()
        {
            print("Hide warning");
            warningController.Hide(true);
        }

        #region Warning Message

        string GetContext(int idx)
        {
            foreach (var cont in scenario.context)
            {
                if (cont.id == idx) return cont.warning;
            }

            return null;
        }

        string GetAmount(int idx)
        {
            foreach (var amt in scenario.amount)
            {
                if (amt.id == idx) return amt.warning;
            }

            return null;
        }

        public string GetRightMessage(int idx, Context context, Amount amount)
        {
            // original warning
            var message = GetIncident(idx).Warning;

            if (context == Context.Irrelevant)
            {
                var t = GetContext(idx);
                if (t == null)
                {
                    Debug.LogWarning($"Failed to load irrelevant warning for {_name} - {idx}");
                }
                else
                {
                    message = t;
                }
            }

            if (amount == Amount.Overload)
            {
                var t = GetAmount(idx);
                if (t == null)
                {
                    Debug.LogWarning($"Failed to load overload warning for {_name} - {idx}.");
                }
                else
                {
                    message = t;
                }
            }

            return message;
        }

        #endregion

        #region Simulation

        void StartIncident(int obj)
        {
            if (obj > incidents.Count)
            {
                Debug.LogWarning($"No incident # {obj} for {ClsName}");
                return;
            }

            OnIncidentStart(obj);
        }

        void EndIncident(int obj)
        {
            if (obj > incidents.Count)
            {
                Debug.LogWarning($"No incident # {obj} for {ClsName}");
                return;
            }

            OnIncidentFinish(obj);
        }

        #endregion

        #region Task Instruction

        public virtual void UpdateInstruction()
        {
            if (_taskStart > 0 && _taskEnd > 0)
            {
                scenariosManager.UpdateInstruction(_taskStart, _taskEnd);
            }
        }


        #endregion
    }
}