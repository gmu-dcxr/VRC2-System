using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;

namespace VRC2.Menu {

    public enum MenuItem
    {
        Zero = 0,
        PickAPipe = 1,
        CheckPipe = 2,
    }

    internal static class MenuString
    {
        public static string empty = "";
        public static string PickAPipe = "Pick A Pipe";
        public static string CheckPipe = "Check Pipe";
    }

    public class MenuInitializer
    {

        private IDictionary<MenuItem, string> menuItemNames;
        private IDictionary<string, MenuItem> nameMenuItems;

        private List<MenuItem> P1MenuItems = new List<MenuItem>() { MenuItem.PickAPipe };
        private List<MenuItem> P2MenuItems = new List<MenuItem>() { MenuItem.CheckPipe };

        public List<MenuItem> P1Menu
        {
            get { return P1MenuItems; }
        }

        public List<MenuItem> P2Meu
        {
            get { return P2MenuItems; }
        }
        
        
        public MenuInitializer()
        {
            menuItemNames = new Dictionary<MenuItem, string>();
            nameMenuItems = new Dictionary<string, MenuItem>();
            // add menu items

            // pick a pipe
            menuItemNames.Add(MenuItem.PickAPipe, MenuString.PickAPipe);
            nameMenuItems.Add(MenuString.PickAPipe, MenuItem.PickAPipe);

            // check pipe
            menuItemNames.Add(MenuItem.CheckPipe, MenuString.CheckPipe);
            nameMenuItems.Add(MenuString.CheckPipe, MenuItem.CheckPipe);
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

    public class MenuController : MonoBehaviour
    {
        [Header("Menu")] [SerializeField] private GameObject _menuRoot;

        
        public bool IsP1 = true;

        private MenuInitializer _menuInitializer = null;
        // Start is called before the first frame update
        void Start()
        {
            _menuInitializer = new MenuInitializer();
            // initialize menu
            InitializeMenuText();
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
        
        # region Initialize Menu Text

        void InitializeMenuText()
        {
            var allTextGameObject = Utils.GetChildren<TextMeshPro>(_menuRoot);
            var gameObjectCount = allTextGameObject.Count;

            List<MenuItem> items = null;
            if (IsP1)
            {
                items = _menuInitializer.P1Menu;
            }
            else
            {
                items = _menuInitializer.P2Meu;
            }

            var count = 0;
            foreach (var item in items)
            {
                var s = _menuInitializer.getStringByMenuItem(item);
                if (count >= gameObjectCount) break;

                allTextGameObject[count].GetComponent<TextMeshPro>().text = s;
            }

        }
        
        #endregion
    }
}
