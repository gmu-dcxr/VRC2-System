using System;
using UnityEngine;

using VRC2.Utility;

namespace VRC2.Task
{
    public class Base : MonoBehaviour
    {
        [Header("Filename")] [Tooltip("Filename under Assets/Conf, e.g. Task/Training.yml")]
        public string filename;


        private void Start()
        {
            ParseYmlFile();
        }

        public void ParseYmlFile()
        {
            var path = Helper.GetConfigureFile(Application.dataPath, filename);

            var task = Helper.ParseYamlFile<YamlParser.Task>(path);
            print(task);
        }
    }
}