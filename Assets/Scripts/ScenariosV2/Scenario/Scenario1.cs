using System;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario1: Base
    {
        private void Start()
        {
            var filename = "Scenario1.yml";
            ParseYamlFile(filename);
        }
    }
}