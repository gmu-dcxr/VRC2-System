using UnityEngine;
using VRC2.Scenarios;

namespace Scenarios.ScenarioFactory
{
    public class BaselineS1: Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS1.yml";
        private void Start()
        {
            InitFromFile(filename);
        }
    }
}