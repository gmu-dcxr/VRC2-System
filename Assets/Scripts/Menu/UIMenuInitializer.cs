using System;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Utility;
using VRC2.Scenarios;
using YamlDotNet.Serialization;
using MenuFile = VRC2.Menu.YamlParser.MenuFile;
using UIMenu = VRC2.Menu.YamlParser.Menu;

namespace VRC2.Menu
{
    public class UIMenuInitializer : MonoBehaviour
    {
        #region Input file

        [Header("Filename")] 
        [Tooltip("The full name under Conf/, e.g., Menu/a.yml")]
        public string filename;

        #endregion

        #region Parsed

        private MenuFile menuFile;
        private UIMenu rootMenu;

        public string RootMenu
        {
            get => rootMenu.text;
        }

        private List<UIMenu> menuStack;


        #endregion

        [HideInInspector] public string BackString = "Back";


        private void Start()
        {
            ParseYamlFile(filename);
            
            // initialize menustack
            menuStack = new List<UIMenu>();
            menuStack.Add(rootMenu);
        }

        public void EnstackMenu(string name)
        {
            // get header
            var header = GetCurrentMenu();
            // sub menu
            var submenu = GetSubMenuByName(header, name);
            menuStack.Add(submenu);
        }

        public UIMenu GetCurrentMenu()
        {
            var count = menuStack.Count;
            return menuStack[count - 1];
        }

        public void ParseYamlFile(string name)
        {
            var path = Helper.GetConfigureFile(Application.dataPath, name);
            menuFile = Helper.ParseYamlFile<YamlParser.MenuFile>(path);
            rootMenu = menuFile.menu;
        }

        public UIMenu GetSubMenuByName(UIMenu menu, string name)
        {
            if (menu.menu == null) return null;

            foreach (var m in menu.menu)
            {
                if (m.text == name) return m;
            }

            return null;
        }

        public List<UIMenu> GetSubMenu(UIMenu menu)
        {
            return menu.menu;
        }

        public void OnBack()
        {
            // clear all and re-add
            menuStack.Clear();
            menuStack.Add(rootMenu);
        }

        public bool IsLeaf(UIMenu menu)
        {
            return menu.menu == null;
        }

        public List<string> GetSubMenuNames(UIMenu menu)
        {
            var subs = menu.menu;
            if (subs == null) return null;
            List<string> names = new List<string>();
            for (var i = 0; i < subs.Count; i++)
            {
                names.Add(subs[i].text);
            }
            
            // add Back at last
            names.Add(BackString);

            return names;
        }

        public List<string> GetSubMenuNames()
        {
            return GetSubMenuNames(rootMenu);
        }
    }
}