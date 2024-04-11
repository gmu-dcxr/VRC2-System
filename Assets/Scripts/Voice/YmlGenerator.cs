using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AiKodexDeepVoice;
using UnityEngine;
using VRC2.Utility;
using VRC2.Scenarios;
using VRC2.ScenariosV2.Base;
using YamlDotNet.Serialization;
using System.Reflection;

namespace VRC2.Voice
{
    public class YmlGenerator : MonoBehaviour
    {
        [Header("Input")] public string filename;
        public List<string> keyPath;
        public string keyString;
        public string keyText;

        [Space(30)] [Header("Output")] public string folder;

        [Space(30)] [Header("UI")] public CanvasController canvasController;

        // isScenario == true: use YamlHelper.Scenario for parsing yaml
        // isScenario == false: use YamlParser.Vehicle for parsing yaml
        [Space(30)] [Header("Type")] public bool isScenario = false;

        private void Start()
        {
            canvasController.invoice.text = "IN010002901947";
        }

        private string GetAudioFileName(string folder, List<string> keypath, object key)
        {
            var prefix = filename.Split('.')[0];
            var name = $"{prefix}_{String.Join("_", keypath)}_{key}";
            name += ".wav";
            return Path.Combine(folder, name);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "YmlGenerator"))
            {
                StartCoroutine(Generate());
            }
        }

        IEnumerator Generate()
        {
            var fullFolder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;

            var path = Helper.GetConfigureFile(Application.dataPath, filename);

            print(path);

            object data = null;
            Type type = null;

            if (isScenario)
            {
                data = (object)Helper.ParseYamlFile<YamlHelper.Scenario>(path);
                type = typeof(YamlHelper.Scenario);
            }
            else
            {
                data = (object)Helper.ParseYamlFile<YamlParser.Vehicle>(path);
                type = typeof(YamlParser.Vehicle);

            }

            print(data);

            object src = data;
            object value = null;
            for (var i = 0; i < keyPath.Count; i++)
            {
                var property = type.GetProperty(keyPath[i]);
                src = property.GetValue(src);
                type = src.GetType();
            }

            type = src.GetType();
            if (type.IsGenericType)
            {
                var enumerable = src as IEnumerable;
                foreach (var item in enumerable)
                {
                    var key = item.GetType().GetProperty(keyString).GetValue(item);
                    var text = (string)item.GetType().GetProperty(keyText).GetValue(item);

                    if (text == null || text.Length < 1) continue;

                    var audioname = GetAudioFileName(fullFolder, keyPath, key);

                    canvasController.text.text = text;
                    canvasController.fileName.text = audioname;

                    yield return new WaitForSecondsRealtime(1);

                    canvasController.Generate();

                    yield return new WaitUntil(() => canvasController.generate.interactable);

                    canvasController.Save();

                    //Wait for 2 seconds
                    yield return new WaitForSecondsRealtime(2);
                    print($" {key} was saved to {audioname}");
                }
            }

            Debug.Log("All Done");
        }
    }
}