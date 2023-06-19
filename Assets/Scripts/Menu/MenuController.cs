using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

namespace VRC2.Menu
{

    public enum MenuItem
    {
        Zero = 0,
        PickAPipe = 1,
        CheckPipe = 2,
        MeasureDistance = 3,
        CommandRobot = 4,
    }

    internal static class MenuString
    {
        public static string empty = "";
        public static string PickAPipe = "Pick A Pipe";
        public static string CheckPipe = "Check Pipe";
        public static string MeasureDistance = "Measure Distance";
        public static string CommandRobot = "Command Robot";
    }

    public class MenuInitializer
    {

        private IDictionary<MenuItem, string> menuItemNames;
        private IDictionary<string, MenuItem> nameMenuItems;

        private List<MenuItem> P1MenuItems = new List<MenuItem>() { MenuItem.PickAPipe };
        private List<MenuItem> P2MenuItems = new List<MenuItem>() { MenuItem.CheckPipe, 
            MenuItem.MeasureDistance,
            MenuItem.CommandRobot
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

            // pick a pipe
            AddPair(MenuItem.PickAPipe, MenuString.PickAPipe);
            // check pipe
            AddPair(MenuItem.CheckPipe, MenuString.CheckPipe);
            // measure distance
            AddPair(MenuItem.MeasureDistance, MenuString.MeasureDistance);
            // command robot
            AddPair(MenuItem.CommandRobot, MenuString.CommandRobot);
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

        public bool IsP1 = true;


        private MenuInitializer _menuInitializer = null;

        private PipeMenuHandler _menuHandler = null;

        // Start is called before the first frame update
        void Start()
        {
            _menuInitializer = new MenuInitializer();

            _menuHandler = gameObject.GetComponent<PipeMenuHandler>();
            // initialize menu
            InitializeMenuText(leaveUnusedBlank);
            // initialize action
            InitializeMenuAction();
        }

        // Update is called once per frame
        void Update()
        {

        }

        # region Initialize Menu

        // blank_remaining: leave unused menus blank or not
        void InitializeMenuText(bool blank_remaining)
        {
            var allTextGameObject = Utils.GetChildren<TextMeshPro>(_menuRoot);

            List<MenuItem> items = null;

            if (IsP1)
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
                    case MenuItem.PickAPipe:
                        puew.WhenRelease.AddListener(_menuHandler.OnPickAPipe);
                        break;
                    case MenuItem.CheckPipe:
                        puew.WhenRelease.AddListener(_menuHandler.OnCheckPipe);
                        break;
                    case MenuItem.MeasureDistance:
                        puew.WhenRelease.AddListener(_menuHandler.OnMeasureDistance);
                        break;
                    case MenuItem.CommandRobot:
                        puew.WhenRelease.AddListener(_menuHandler.OnCommandRobot);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
