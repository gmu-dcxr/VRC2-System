using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRC2
{
    public static class Utils
    {
        public static List<GameObject> GetChildren(GameObject go)
        {
            List<GameObject> list = new List<GameObject>();
            return GetChildrenHelper(go, list);
        }

        private static List<GameObject> GetChildrenHelper(GameObject go, List<GameObject> list)
        {
            if (go == null || go.transform.childCount == 0)
            {
                return list;
            }

            foreach (Transform t in go.transform)
            {
                list.Add(t.gameObject);
                GetChildrenHelper(t.gameObject, list);
            }

            return list;
        }

        public static List<GameObject> GetChildren<T>(GameObject go)
        {
            List<GameObject> list = new List<GameObject>();
            list = GetChildrenHelper(go, list);
            List<GameObject> result = new List<GameObject>();
            foreach (var item in list)
            {
                if (item.GetComponent<T>() != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        #region Get Enum Variable String

        public static string GetDisplayName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }

        #endregion

        #region Find Object with The Same Name

        public static IEnumerable<GameObject> FindAll(string name)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == name);
            return objects;
        }
        

        #endregion
    }
}