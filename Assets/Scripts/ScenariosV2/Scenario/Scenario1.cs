using System;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario1: Base
    {
        private void Start()
        {
            _id = 1;
            
            var filename = "Scenario1.yml";
            ParseYamlFile(filename);
        }
    }
}