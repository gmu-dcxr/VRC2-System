using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Network;
using VRC2.SAGAT;
using VRC2.Utility;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Tool;
using VRC2.ScenariosV2.Vehicle;
using YamlDotNet.Serialization;
using Incident = VRC2.ScenariosV2.Base.Incident;

using TaskBase = VRC2.Task.Base;
using YamlParser = VRC2.ScenariosV2.Base.YamlParser;

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
        private bool lastSurvey = true;

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

        [Space(30)] [Header("Prepared Pipes")] public List<GameObject> preparedPipes;

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

        // last sagat incident
        public System.Action SAGATStart;

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

        // scenario 7
        private Vehicle.ErroneousAI _erroneousAI;

        // survey
        private Vehicle.Survey _survey;

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

        public Vehicle.ErroneousAI erroneousAI
        {
            get
            {
                if (_erroneousAI == null)
                {
                    _erroneousAI = FindObjectOfType<Vehicle.ErroneousAI>();
                }

                return _erroneousAI;
            }
        }

        public Vehicle.Survey survey
        {
            get
            {
                if (_survey == null)
                {
                    _survey = FindObjectOfType<Vehicle.Survey>();
                }

                return _survey;
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

        private SagatController _sagatController;

        public SagatController sagatController
        {
            get
            {
                if (_sagatController == null)
                {
                    _sagatController = FindFirstObjectByType<SagatController>();
                }

                return _sagatController;
            }
        }

        // change order
        private List<int> changeOrderIndices = new List<int>();

        #endregion

        [Header("Debug UI")] public bool showDebugUI = true;
        public bool scenarioButtonOnly = false;
        private bool showTime = true;
        public bool showTimeline = false;
        private string timelineText = "";
        private int timelineSecond = 0;
        private GUIStyle backgroundStyle;

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
            lastSurvey = s.lastSurvey;
            print($"last survey: {lastSurvey}");

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

            // erroneous AI
            if (cname.Equals(erroneousAI.ClsName))
            {
                res = erroneousAI.GetAccidentIncident(idx);
            }

            try
            {
                // survey
                if (cname.Equals(survey.ClsName))
                {
                    res = survey.GetIncident(idx, normal);
                }
            }
            catch (Exception e)
            {
                ;
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
                // update index, time defined in scenario
                inci.AddEntry(i, _incidents[i].time);

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
            this.SAGATStart += OnSagatStart;
        }

        private void OnSagatStart()
        {
            HideTimeline();
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

        private void UpdatePreparedPipes()
        {
            var ss = scenariosManager.scenarios;
            foreach (var s in ss)
            {
                if (s.Equals(this))
                {
                    if (s.preparedPipes == null) continue;

                    foreach (var pipe in s.preparedPipes)
                    {
                        pipe.SetActive(true);
                    }
                }
                else
                {
                    if (s.preparedPipes == null) continue;
                    foreach (var pipe in s.preparedPipes)
                    {
                        pipe.SetActive(false);
                    }
                }
            }
        }

        public void Execute(int timestamp)
        {
            StartScenario(timestamp);
            // start by clicking button on GUI
            if (startInSec == 0)
            {
                OnScenarioStart();
            }
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
            // update timeline second
            timelineSecond = sec;

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

                    // show timeline
                    if (showTimeline)
                    {
                        FormatTimelineText(null);
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

            // in some cases timeIncidentIdxMap could be null if there is no incident in some scenario
            if (timeIncidentIdxMap != null && timeIncidentIdxMap.ContainsKey(sec) && !startedIncidents.Contains(sec))
            {
                // idx
                var indices = timeIncidentIdxMap[sec];

                // show timeline
                if (showTimeline)
                {
                    FormatTimelineText(indices);
                }

                foreach (var idx in indices)
                {
                    // change order
                    if (changeOrderIndices.Contains(idx))
                    {
                        // trigger change order
                        StartCoroutine(ChangeOrder());
                    }

                    StartCoroutine(CoroutineStartIncident(idx));
                    // last event is a survey
                    if (idx == parsedIncidents.Count - 1 && lastSurvey)
                    {
                        // assume it's sagat
                        if (SAGATStart != null)
                        {
                            SAGATStart();
                        }
                    }
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

        string FormatIncident(int idx)
        {
            var inci = parsedIncidents[idx];
            var (index, time) = inci.GetEntry(idx);
            return $"#{index + 1}\t{time}\t{inci.callback}";
        }

        void FormatTimelineText(List<int> indices)
        {
            // [0, idx] black
            // idx, red
            // [idx,~], white

            // clear
            timelineText = $"<color=blue><b>Timeline of {ClsName}</b></color>\n" +
                           $"<color=yellow>{startTimeRaw} - {endTimeRaw}</color>\n\n";
            var count = parsedIncidents.Count;

            // at the beginning
            if (indices == null)
            {
                for (var i = 0; i < count; i++)
                {
                    // white
                    timelineText += $"<color=white>{FormatIncident(i)}</color>\n";
                }

                return;
            }

            for (var i = 0; i < indices[0]; i++)
            {
                // black - passed
                timelineText += $"<color=black>{FormatIncident(i)}</color>\n";
            }

            var ic = indices.Count;
            for (var i = 0; i < ic; i++)
            {
                // red - current
                timelineText += $"<color=red>{FormatIncident(indices[i])}</color>\n";
            }

            for (var i = indices[ic - 1] + 1; i < count; i++)
            {
                // white - coming
                timelineText += $"<color=white>{FormatIncident(i)}</color>\n";
            }
        }

        void ShowTimelineSecond()
        {
            if (timelineSecond == 0) return;

            if (backgroundStyle == null)
            {
                backgroundStyle = new GUIStyle();
                Texture2D backgroundTexture = new Texture2D(1, 1);
                Color backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                backgroundTexture.SetPixel(0, 0, backgroundColor);
                backgroundTexture.Apply();
                backgroundStyle.normal.background = backgroundTexture;
            }

            var text = $"<size=20><color=cyan>{(int)(timelineSecond / 60)} : {timelineSecond % 60}</color></size>\n";
            GUILayout.BeginVertical(backgroundStyle, GUILayout.ExpandWidth(true));
            GUILayout.Label(text);
            GUILayout.EndVertical();
        }

        void ShowIncidentTimeline(string richtext)
        {
            if (backgroundStyle == null)
            {
                backgroundStyle = new GUIStyle();
                Texture2D backgroundTexture = new Texture2D(1, 1);
                Color backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                backgroundTexture.SetPixel(0, 0, backgroundColor);
                backgroundTexture.Apply();
                backgroundStyle.normal.background = backgroundTexture;
            }

            GUILayout.BeginVertical(backgroundStyle, GUILayout.ExpandHeight(true));
            GUILayout.Label(richtext, GUILayout.ExpandHeight(true));
            GUILayout.EndVertical();
        }

        void ShowIncidentButtons()
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            // scenario text 
            if (!started && GUILayout.Button($"Start {ClsName}", GUILayout.ExpandHeight(true)))
            {
                var t = Helper.SecondNow();
                StartScenario(t);
                // sync it
                scenariosManager.Sync_StartScenario(ClsName, t);
            }

            if (!scenarioButtonOnly)
            {
                var count = parsedIncidents.Count;
                for (var i = 0; i < count; i++)
                {
                    var text = $"Start {i + 1}";
                    // add change order mark
                    if (changeOrderIndices.Contains(i))
                    {
                        text += " (C)";
                    }

                    if (GUILayout.Button(text))
                    {
                        // change order
                        if (changeOrderIndices.Contains(i))
                        {
                            StartCoroutine(ChangeOrder());
                        }

                        StartIncident(i + 1);
                    }
                }
            }

            GUILayout.EndVertical();
        }

        private void OnGUI()
        {
            if (showTime)
            {
                ShowTimelineSecond();
            }

            if (showTimeline)
            {
                ShowIncidentTimeline(timelineText);
            }

            if (!showDebugUI) return;

            ShowIncidentButtons();
        }

        #endregion

        public virtual void OnScenarioStart()
        {
            print($"Invoke {name} OnScenarioStart");

            // update prepared pipes
            UpdatePreparedPipes();

            // bypass SAGAT errors
            try
            {
                UpdateSAGAT();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            UpdateInstruction();
        }

        public virtual void HideTimeline()
        {
            // hide timeline and ui to let player answer questions
            showTime = false;
            showTimeline = false;
            showDebugUI = false;
            Debug.LogWarning($"[{ClsName}] Disable Timeline and DebugUI");
        }

        public virtual void OnScenarioFinish()
        {
            print($"Invoke {name} OnScenarioFinish");
        }

        public virtual void UpdateSAGAT()
        {
            // title
            sagatController.UpdateScenarioText(ClsName);
            // questions
            sagatController.UpdateQuestions(ClsName);
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
            // default is training, nothing to update
            if (taskStart >= 0 && taskEnd >= 0)
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

            if (taskBase == null)
            {
                Debug.LogError($"[ScenarioV2-Base] Failed to load task: {clsName}");
            }
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