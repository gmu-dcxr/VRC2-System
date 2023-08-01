using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS7 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS7.yml";


        [Header("Player")] public GameObject player;



        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();


        }

        private void Update()
        {

        }


        private void OnIncidentFinish(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);

            print($"[{ClsName}] Callback: {name}");

            Invoke(name, 0);
        }

        private void OnIncidentStart(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);

            print($"[{ClsName}] Callback: {name}");
            Invoke(name, 0);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);
            }
        }


        #region Accident Events Callbacks

        public void On_BaselineS7_1_Start()
        {

        }

        public void On_BaselineS7_1_Finish()
        {

        }

        public void On_BaselineS7_2_Start()
        {
            print("On_BaselineS7_2_Start");
            // An excavator is digging next to the participants.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

        }

        public void On_BaselineS7_2_Finish()
        {
            // An excavator is digging next to the participants.
        }

        public void On_BaselineS7_3_Start()
        {
            print("On_BaselineS7_3_Start");
            // The excavator is digging into the working zone.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);
        }

        public void On_BaselineS7_3_Finish()
        {
            // The excavator is digging into the working zone.

        }

        public void On_BaselineS7_4_Start()
        {
            print("On_BaselineS7_4_Start");
            // The excavator is digging more into the working zone.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

  
        }

        public void On_BaselineS7_4_Finish()
        {
            // The excavator is digging more into the working zone.
        }


        public void On_BaselineS7_5_Start()
        {
            print("On_BaselineS7_5_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS7_5_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}