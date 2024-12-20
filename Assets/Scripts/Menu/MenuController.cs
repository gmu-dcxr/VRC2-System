using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using VRC2.Record;
using VRC2.ScenariosV2.Tool;

namespace VRC2.Menu
{
    public enum MenuItem
    {
        Zero,
        VoiceControl,
        GiveInstruction,
        CheckStorage,
        Deprecate,
        PickupPipe,
        CheckPipeSizeColor,
        MeasureDistance,
        CommandRobot,
        CheckGlue,
        CheckClamp,
        CheckPipeLengthAngle,
        CheckLevel,
        Supervisor,
        SafetyManager,

        // supervisor submenu
        ReportDog,
        ReportInstruction,
        ReportClamp,
        ReportGlue,
        ReportPipe,
        Back, // back to above menu

        // safety manager submenu
        ReportFall,
        ReportStruck,
        ReportElectrified,
        ReportCollision
    }

    internal static class MenuString
    {
        public static string empty = "";
        public static string VoiceControl = "Voice Control";
        public static string GiveInstruction = "Give Instruction";
        public static string CheckStorage = "AI Drone";
        public static string Deprecate = "Deprecate";
        public static string PickupPipe = "Pickup Pipe";
        public static string CheckPipeSizeColor = "Check Size/Color";
        public static string MeasureDistance = "Measure Distance";
        public static string CommandRobot = "RobotDog";
        public static string CheckGlue = "Glue";
        public static string CheckClamp = "Clamp";
        public static string CheckPipeLengthAngle = "Check Length/Angle";

        public static string CheckLevel = "Check Level";

        // newly added
        public static string Supervisor = "Supervisor";
        public static string SafetyManager = "Safety Manager";

        // supervisor level 2 menu
        public static string ReportDog = "Report Dog";
        public static string ReportInstruction = "Report Instruction";
        public static string ReportClamp = "Report Clamp";
        public static string ReportGlue = "Report Glue";
        public static string ReportPipe = "Report Pipe";
        public static string Back = "Back"; // shared by the following

        // safety manager level 2 menu
        public static string ReportFall = "Report Fall";
        public static string ReportStruck = "Report Struck";
        public static string ReportElectrified = "Report Electrified";
        public static string ReportCollision = "Report Collision";

    }

    public class MenuInitializer
    {
        private IDictionary<MenuItem, string> menuItemNames;
        private IDictionary<string, MenuItem> nameMenuItems;

        private List<MenuItem> P1MenuItems = new List<MenuItem>()
        {
            // MenuItem.VoiceControl, // disable
            // MenuItem.CheckStorage,
            // MenuItem.PickupPipe, // disable pickup since the logic is changed
            // MenuItem.Deprecate, // disable 
            // check glue and clamp
            // MenuItem.CheckGlue,
            // MenuItem.CheckClamp,
            // supervisor and safety manager
            MenuItem.Supervisor,
            MenuItem.SafetyManager,
        };

        private List<MenuItem> P2MenuItems = new List<MenuItem>()
        {
            // MenuItem.VoiceControl, // disable
            // MenuItem.GiveInstruction, // disable
            // MenuItem.CheckPipeSizeColor, // disable
            // MenuItem.MeasureDistance, // disable
            MenuItem.CheckStorage,
            MenuItem.CommandRobot,
            MenuItem.CheckGlue,
            MenuItem.CheckClamp,
            // MenuItem.CheckPipeLengthAngle, // disable
            // MenuItem.CheckLevel // disable
            // supervisor and safety manager
            MenuItem.Supervisor,
            MenuItem.SafetyManager,
        };

        private List<MenuItem> SupervisorItems = new List<MenuItem>()
        {
            MenuItem.ReportDog,
            MenuItem.ReportInstruction,
            MenuItem.ReportClamp,
            MenuItem.ReportGlue,
            MenuItem.ReportPipe,
            MenuItem.Back,
        };

        private List<MenuItem> SafetyManagerItems = new List<MenuItem>()
        {
            MenuItem.ReportFall,
            MenuItem.ReportStruck,
            MenuItem.ReportElectrified,
            MenuItem.ReportCollision,
            MenuItem.Back,
        };

        public List<MenuItem> P1Menu
        {
            get => P1MenuItems;
        }

        public List<MenuItem> P2Meu
        {
            get => P2MenuItems;
        }

        public List<MenuItem> SupervisorMenu
        {
            get => SupervisorItems;
        }

        public List<MenuItem> SafetyManagerMenu
        {
            get => SafetyManagerItems;
        }

        internal void AddPair(MenuItem item, string name)
        {
            menuItemNames.Add(item, name);
            nameMenuItems.Add(name, item);
        }


        public MenuInitializer()
        {
            menuItemNames = new Dictionary<MenuItem, string>();
            nameMenuItems = new Dictionary<string, MenuItem>();
            // add menu items

            // voice control
            AddPair(MenuItem.VoiceControl, MenuString.VoiceControl);
            // check storage
            AddPair(MenuItem.CheckStorage, MenuString.CheckStorage);
            // pick a pipe
            AddPair(MenuItem.PickupPipe, MenuString.PickupPipe);
            // deprecate current pipe
            AddPair(MenuItem.Deprecate, MenuString.Deprecate);
            // p2 give p1 instruction
            AddPair(MenuItem.GiveInstruction, MenuString.GiveInstruction);
            // check pipe size and color
            AddPair(MenuItem.CheckPipeSizeColor, MenuString.CheckPipeSizeColor);
            // measure distance
            AddPair(MenuItem.MeasureDistance, MenuString.MeasureDistance);
            // command robot
            AddPair(MenuItem.CommandRobot, MenuString.CommandRobot);
            // check glue
            AddPair(MenuItem.CheckGlue, MenuString.CheckGlue);
            // check clamp
            AddPair(MenuItem.CheckClamp, MenuString.CheckClamp);
            // check pipe length and angle
            AddPair(MenuItem.CheckPipeLengthAngle, MenuString.CheckPipeLengthAngle);
            // check level
            AddPair(MenuItem.CheckLevel, MenuString.CheckLevel);
            // add supervisor
            AddPair(MenuItem.Supervisor, MenuString.Supervisor);
            // add safety manager
            AddPair(MenuItem.SafetyManager, MenuString.SafetyManager);

            // add supervisor submenu
            AddPair(MenuItem.ReportDog, MenuString.ReportDog);
            AddPair(MenuItem.ReportInstruction, MenuString.ReportInstruction);
            AddPair(MenuItem.ReportClamp, MenuString.ReportClamp);
            AddPair(MenuItem.ReportGlue, MenuString.ReportGlue);
            AddPair(MenuItem.ReportPipe, MenuString.ReportPipe);
            AddPair(MenuItem.Back, MenuString.Back);
            // add safety manager submenu
            AddPair(MenuItem.ReportFall, MenuString.ReportFall);
            AddPair(MenuItem.ReportStruck, MenuString.ReportStruck);
            AddPair(MenuItem.ReportElectrified, MenuString.ReportElectrified);
            AddPair(MenuItem.ReportCollision, MenuString.ReportCollision);
        }

        public MenuItem getMenuItemByString(string name)
        {
            MenuItem item;
            if (nameMenuItems.TryGetValue(name, out item))
                return item;
            return MenuItem.Zero;
        }

        public string getStringByMenuItem(MenuItem item)
        {
            string res = null;
            if (menuItemNames.TryGetValue(item, out res))
                return res;
            return null;
        }
    }

    [RequireComponent(typeof(PipeMenuHandler))]
    public class MenuController : MonoBehaviour
    {
        [Header("Menu")] [SerializeField] private GameObject _menuRoot;

        [Space(30)] [Header("Settings")] [SerializeField]
        private bool leaveUnusedBlank;

        private MenuInitializer _menuInitializer = null;

        private PipeMenuHandler _menuHandler = null;

        private bool _menuInitialized = false;

        private List<MenuItem> _initializedMenuItems = null;

        private DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Space(30)] [Header("Runtime Menus")] public UIMenuInitializer safetyManagerMenu;
        public UIMenuInitializer supervisorMenu;

        [Space(30)] [Header("Report Log")] [ReadOnly]
        public string folder = "../Report";

        [ReadOnly] public string fullFolder;
        [ReadOnly] public string filename;
        [ReadOnly] public string fullpath;

        // Start is called before the first frame update
        void Start()
        {
            _menuInitializer = new MenuInitializer();

            _menuHandler = gameObject.GetComponent<PipeMenuHandler>();

            InitReportLog();
        }

        void InitReportLog()
        {
            fullFolder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;
            filename = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log";
            fullpath = Path.Combine(fullFolder, filename);

            print($"[Report] Use Log: {fullpath}");
        }

        // Update is called once per frame
        void Update()
        {
            if (_menuInitialized || !GlobalConstants.GameStarted)
                return;
            // initialize menu and actions after game started
            if (!_menuInitialized)
            {
                // initialize menu
                InitializeMenuText(leaveUnusedBlank);
                // initialize action
                InitializeMenuAction();
                _menuInitialized = true;
            }
        }

        #region Initialize Menu

        #region Safety Manager Menu

        void SetButtonVisual(GameObject go, bool enable)
        {
            var rbp = go.GetComponentInChildren<RoundedBoxProperties>();
            rbp.GetComponent<MeshRenderer>().enabled = enable;
        }

        void InitializeMenuText(List<string> items, bool blank_remaining)
        {
            var allPokeVisuals = Utils.GetChildren<PokeInteractableVisual>(_menuRoot);
            var count = 0;

            foreach (var go in allPokeVisuals)
            {
                var tmp = go.GetComponentInChildren<TextMeshPro>();
                if (count >= items.Count)
                {
                    if (blank_remaining)
                    {
                        tmp.text = "";
                        // disable button visual
                        SetButtonVisual(go, false);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    var s = items[count];
                    // enable button visual
                    SetButtonVisual(go, true);
                    tmp.text = s;
                }

                count += 1;
            }
        }



        #endregion


        void InitializeMenuText(List<MenuItem> items, bool blank_remaining)
        {
            var allPokeVisuals = Utils.GetChildren<PokeInteractableVisual>(_menuRoot);
            var count = 0;

            foreach (var go in allPokeVisuals)
            {
                var tmp = go.GetComponentInChildren<TextMeshPro>();

                if (count >= items.Count)
                {
                    if (blank_remaining)
                    {
                        tmp.text = "";
                        // disable button visual
                        SetButtonVisual(go, false);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    var s = _menuInitializer.getStringByMenuItem(items[count]);
                    SetButtonVisual(go, true);
                    tmp.text = s;
                }

                count += 1;
            }
        }

        // blank_remaining: leave unused menus blank or not
        void InitializeMenuText(bool blank_remaining)
        {
            if (GlobalConstants.Checkee)
            {
                // P1 Menu
                _initializedMenuItems = _menuInitializer.P1Menu;
            }
            else
            {
                // P2 Menu
                _initializedMenuItems = _menuInitializer.P2Meu;
            }

            InitializeMenuText(_initializedMenuItems, blank_remaining);
        }

        void RevertMainMenuText()
        {
            InitializeMenuText(_initializedMenuItems, leaveUnusedBlank);
        }

        void InitializeMenuAction()
        {
            var allTextGameObject = Utils.GetChildren<TextMeshPro>(_menuRoot);
            foreach (var go in allTextGameObject)
            {
                var text = go.GetComponent<TextMeshPro>();

                // get menu item
                var item = _menuInitializer.getMenuItemByString(text.text);

                if (item == MenuItem.Zero) break;

                // in hierarchy, TMP - parent (ButtonVisual) - parent (Visuals) - parent (Menu x)
                var menu = go.transform.parent.parent.parent.gameObject;

                var puew = menu.GetComponent<PointableUnityEventWrapper>();

                // remove all listeners first
                puew.WhenRelease.RemoveAllListeners();
                switch (item)
                {
                    case MenuItem.VoiceControl:
                        puew.WhenRelease.AddListener(_menuHandler.OnVoiceControl);
                        break;
                    case MenuItem.GiveInstruction:
                        puew.WhenRelease.AddListener(_menuHandler.OnGiveInstruction);
                        break;
                    case MenuItem.CheckStorage:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckStorage);
                        break;
                    case MenuItem.Deprecate:
                        puew.WhenRelease.AddListener(_menuHandler.OnDeprecate);
                        break;
                    case MenuItem.PickupPipe:
                        puew.WhenRelease.AddListener(_menuHandler.OnPickupPipe);
                        break;
                    case MenuItem.CheckPipeSizeColor:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckPipeSizeColor);
                        break;
                    case MenuItem.MeasureDistance:
                        puew.WhenRelease.AddListener(_menuHandler.OnMeasureDistance);
                        break;
                    case MenuItem.CommandRobot:
                        puew.WhenRelease.AddListener(_menuHandler.OnCommandRobot);
                        break;
                    case MenuItem.CheckGlue:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckGlue);
                        break;
                    case MenuItem.CheckClamp:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckClamp);
                        break;
                    case MenuItem.CheckPipeLengthAngle:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckLengthAngle);
                        break;
                    case MenuItem.CheckLevel:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckLevel);
                        break;
                    case MenuItem.Supervisor:
                        puew.WhenRelease.AddListener(OnSupervisor);
                        break;
                    case MenuItem.SafetyManager:
                        puew.WhenRelease.AddListener(OnSafetyManager);
                        break;
                    case MenuItem.ReportDog:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportDog);
                        break;
                    case MenuItem.ReportInstruction:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportInstruction);
                        break;
                    case MenuItem.ReportClamp:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportClamp);
                        break;
                    case MenuItem.ReportGlue:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportGlue);
                        break;
                    case MenuItem.ReportPipe:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportPipe);
                        break;
                    case MenuItem.ReportFall:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportFall);
                        break;
                    case MenuItem.ReportStruck:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportStruck);
                        break;
                    case MenuItem.ReportElectrified:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportElectrified);
                        break;
                    case MenuItem.ReportCollision:
                        puew.WhenRelease.AddListener(_menuHandler.OnReportCollision);
                        break;
                    case MenuItem.Back:
                        puew.WhenRelease.AddListener(OnBack);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion


        #region SubMenus Actions -- Deprecated

        public void OnSupervisor()
        {
            print("OnSupervisor");
            var items = supervisorMenu.GetSubMenuNames();
            InitializeMenuText(items, leaveUnusedBlank);
            InitializeUIMenuInitializerAction(supervisorMenu, items.Count);
        }

        public void OnSafetyManager()
        {
            print("OnSafetyManager");
            var items = safetyManagerMenu.GetSubMenuNames();
            InitializeMenuText(items, leaveUnusedBlank);
            InitializeUIMenuInitializerAction(safetyManagerMenu, items.Count);
        }

        public void InitializeUIMenuInitializerAction(UIMenuInitializer menu, int count)
        {
            var allTextGameObject = Utils.GetChildren<TextMeshPro>(_menuRoot);
            var c = 0;
            foreach (var go in allTextGameObject)
            {
                if (c >= count) break;

                var text = go.GetComponent<TextMeshPro>();
                // in hierarchy, TMP - parent (ButtonVisual) - parent (Visuals) - parent (Menu x)

                var btn = go.transform.parent.parent.parent.gameObject;

                var puew = btn.GetComponent<PointableUnityEventWrapper>();
                // remove all listeners first
                puew.WhenRelease.RemoveAllListeners();
                puew.WhenRelease.AddListener(() => OnUIMenuReleased(menu, text.text));

                c += 1;
            }
        }

        public void OnUIMenuReleased(UIMenuInitializer menu, string text)
        {
            print($"OnUIMenuReleased: {text}");

            if (text.Equals(menu.BackString))
            {
                menu.OnBack();
                OnBack();
                return;
            }

            menu.EnstackMenu(text);
            var cur = menu.GetCurrentMenu();
            if (menu.IsLeaf(cur))
            {
                print($"Leaf menu: {cur.desc}");
                WriteLog(menu, cur);
                // unstack
                menu.UnStackMenu();
            }
            else
            {
                var items = safetyManagerMenu.GetSubMenuNames(cur);
                InitializeMenuText(items, leaveUnusedBlank);
                InitializeUIMenuInitializerAction(menu, items.Count);
            }

            if (cur.action != null && cur.action.Count > 1)
            {
                print($"Customized action for {text}");
                // customized action
                var cls = cur.action[0];
                var method = cur.action[1];

                // invoke using reflection
                var myClassType = Type.GetType(cls);
                var obj = FindObjectOfType(myClassType);

                var mi = myClassType.GetMethod(method);
                mi.Invoke(obj, null);
            }
        }

        void OnBack()
        {
            print("OnBack");

            // call to stop voice recording
            _menuHandler.OnBack();

            RevertMainMenuText();
            InitializeMenuAction();
        }

        void WriteLog(UIMenuInitializer menu, VRC2.Menu.YamlParser.Menu cur)
        {
            // time in real
            var realTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            // Get the time in seconds since the start of the game
            float timeAfterStart = Time.time;
            // Convert the UTC time to milliseconds since the Unix epoch (January 1, 1970)
            long unixTime = (long)(DateTime.UtcNow - unixStart).TotalMilliseconds;

            var stack = menu.MenuStackToString(" - ");

            using (TextWriter writer = File.AppendText(fullpath))
            {
                // write text
                // time, time, time, stack, desc
                writer.WriteLine($"{realTime},{timeAfterStart},{unixTime}," +
                                 $"{stack}," +
                                 $"{cur.desc}");
            }
        }

        #endregion
    }
}