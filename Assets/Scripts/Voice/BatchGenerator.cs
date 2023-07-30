using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AiKodexDeepVoice;
using UnityEngine;
using VRC2.Scenarios;
using YamlDotNet.Serialization;

namespace VRC2.Voice
{
    public class BatchGenerator : MonoBehaviour
    {
        public CanvasController canvasController;


        private void Start()
        {
            canvasController.invoice.text = "IN010002901947";
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Generate"))
            {
                StartCoroutine(Generate());
            }
        }

        IEnumerator Generate()
        {
            var folder = "Audio/VRC2";
            folder = Directory.CreateDirectory(Path.Combine(Application.dataPath, folder)).FullName;

            for (int i = 1; i <= 8; i++)
            {
                // read all yml files
                var name = $"BaselineS{i}.yml";
                var path = Helper.GetConfigureFile(Application.dataPath, name);
                print(path);

                var text = System.IO.File.ReadAllText(path);
                print(text);
                var deser = new DeserializerBuilder().Build();
                var scenairo = deser.Deserialize<YamlHelper.Scenario>(text);
                print(scenairo);

                foreach (var incident in scenairo.incidents)
                {
                    var warning = incident.warning;
                    if (warning == "") continue;

                    var output = $"{scenairo.name}_{incident.id}.wav";
                    // var fullpath = Path.Combine(folder, output);
                    
                    print($"{scenairo.name} - {incident.id} - {warning}");

                    canvasController.text.text = warning;
                    canvasController.fileName.text = output;
                    
                    yield return new WaitForSecondsRealtime(1);

                    canvasController.Generate();

                    Debug.Log("Waiting for generating to be done");
                    yield return new WaitUntil(() => canvasController.generate.interactable);
                    Debug.Log("Generating is done now, save file");

                    canvasController.Save();

                    //Wait for 2 seconds
                    yield return new WaitForSecondsRealtime(2);
                }
            }
            
            Debug.Log("All Done");
        }
    }
}