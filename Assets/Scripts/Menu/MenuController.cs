using System;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

namespace VRC2.Menu
{
    public enum MenuItem
    {
        Zero = 0,
        VoiceControl = 1001,
        GiveInstruction = 1,
        CheckStorage = 11,
        Deprecate = 12,
        PickupPipe = 2,
        CheckPipeSizeColor = 3,
        MeasureDistance = 4,
        CommandRobot = 5,
        CheckGlue = 51,
        CheckClamp = 52,
        CheckPipeLengthAngle = 6,
        CheckLevel = 7,
        Supervisor = 8,
        SafetyManager = 9,
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
    }

    public class MenuInitializer
    {
        private IDictionary<MenuItem, string> menuItemNames;
        private IDictionary<string, MenuItem> nameMenuItems;

        private List<MenuItem> P1MenuItems = new List<MenuItem>()
        {
            // MenuItem.VoiceControl, // disable
            MenuItem.CheckStorage,
            // MenuItem.PickupPipe, // disable pickup since the logic is changed
            // MenuItem.Deprecate, // disable 
            // check glue and clamp
            MenuItem.CheckGlue,
            MenuItem.CheckClamp,
            // supervisor and safety manager
            MenuItem.Supervisor,
            MenuItem.SafetyManager,
        };

        private List<MenuItem> P2MenuItems = new List<MenuItem>()
        {
            // MenuItem.VoiceControl, // disable
            // MenuItem.GiveInstruction, // disable
            // MenuItem.CheckPipeSizeColor, // disable
            MenuItem.MeasureDistance,
            MenuItem.CommandRobot,
            // MenuItem.CheckPipeLengthAngle, // disable
            // MenuItem.CheckLevel // disable
            // supervisor and safety manager
            MenuItem.Supervisor,
            MenuItem.SafetyManager,
        };

        public List<MenuItem> P1Menu
        {
            get { return P1MenuItems; }
        }

        public List<MenuItem> P2Meu
        {
            get { return P2MenuItems; }
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

        [Header("Settings")] [SerializeField] private bool leaveUnusedBlank;

        private MenuInitializer _menuInitializer = null;

        private PipeMenuHandler _menuHandler = null;

        private bool _menuInitialized = false;

        // Start is called before the first frame update
        void Start()
        {
            _menuInitializer = new MenuInitializer();

            _menuHandler = gameObject.GetComponent<PipeMenuHandler>();
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

        # region Initialize Menu

        // blank_remaining: leave unused menus blank or not
        void InitializeMenuText(bool blank_remaining)
        {
            var allTextGameObject = Utils.GetChildren<TextMeshPro>(_menuRoot);

            List<MenuItem> items = null;

            if (GlobalConstants.Checkee)
            {
                // P1 Menu
                items = _menuInitializer.P1Menu;
            }
            else
            {
                // P2 Menu
                items = _menuInitializer.P2Meu;
            }

            var count = 0;

            foreach (var go in allTextGameObject)
            {
                var tmp = go.GetComponent<TextMeshPro>();
                if (count >= items.Count)
                {
                    if (blank_remaining)
                    {
                        tmp.text = "";
                        // disable it
                        try
                        {
                            // some TextMeshPros are not menu.
                            go.transform.parent.parent.gameObject.SetActive(false);
                        }
                        catch (Exception e)
                        {
                            ;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    var s = _menuInitializer.getStringByMenuItem(items[count]);
                    tmp.text = s;
                }

                count += 1;
            }
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
                        puew.WhenRelease.AddListener(_menuHandler.OnSupervisor);
                        break;
                    case MenuItem.SafetyManager:
                        puew.WhenRelease.AddListener(_menuHandler.OnSafetyManager);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}