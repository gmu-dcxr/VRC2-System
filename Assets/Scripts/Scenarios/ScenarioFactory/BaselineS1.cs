using System;
using UnityEngine;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS1: Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS1.yml";

        private void Start()
        {
            base.Start();
            
            InitFromFile(filename);
            
            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;
            
            CheckIncidentsCallbacks();
        }

        private void OnIncidentFinish(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);

            print($"[{ClsName}] Callback: {name}");
        }

        private void OnIncidentStart(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);
            
            print($"[{ClsName}] Callback: {name}");
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);
            }
        }

        public void On_BaselineS1_1_Start()
        {
            
        }

        public void On_BaselineS1_1_Finish()
        {
            
        }
    }
}