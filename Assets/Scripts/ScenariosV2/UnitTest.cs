using UnityEngine;
using Helper = VRC2.Scenarios.Helper;
using VRC2.ScenariosV2.Base;
using YamlDotNet.Serialization;

namespace VRC2.ScenariosV2
{
    public class UnitTest: MonoBehaviour
    {
        private void Start()
        {
            var name = "Crane.yml";
            // var path = Helper.GetConfigureFile(Application.dataPath, name);
            // print(path);
            //
            // var text = System.IO.File.ReadAllText(path);
            // print(text);
            //
            // var deser = new DeserializerBuilder().Build();
            // var Vehicle = deser.Deserialize<YamlParser.Vehicle>(text);
            // print(Vehicle);

            var b = gameObject.AddComponent<Vehicle.Crane>();
            b.ParseYamlFile(name);
            print(b.GetIncident(1, true));
        }
    }
}