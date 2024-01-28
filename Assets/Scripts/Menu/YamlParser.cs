using System.Collections.Generic;
using JetBrains.Annotations;

namespace VRC2.Menu
{
    public class YamlParser
    {

        #region Menu

        public class Menu
        {
            public int id { get; set; }
            public int level { get; set; }
            public string text { get; set; }
            [CanBeNull] public string desc { get; set; }
            [CanBeNull] public List<Menu> menu { get; set; }
        }

        #endregion

        #region File

        public class MenuFile
        {
            public string name { get; set; }
            [CanBeNull] public string desc { get; set; }
            public Menu menu { get; set; }
        }

        #endregion
    }
}