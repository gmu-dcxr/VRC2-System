using System;
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

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);
            }
        }
    }
}