using System;
using UnityEngine;

using YamlDotNet;
using YamlDotNet.Serialization;

namespace VRC2.Scenarios
{
    public class ScenerioDev: MonoBehaviour
    {
        private void Start()
        {
            var name = "BaselineS1.yml";
            var path = Helper.GetConfigureFile(Application.dataPath, name);
            print(path);

            var text = System.IO.File.ReadAllText(path);
            print(text);

            var deser = new DeserializerBuilder().Build();
            var scenairo = deser.Deserialize<YamlHelper.Scenario>(text);
            print(scenairo);
        }
    }
}