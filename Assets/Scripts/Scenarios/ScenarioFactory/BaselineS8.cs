using NodeCanvas.BehaviourTrees;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS8 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS8.yml";

        [Header("TruckPositions")]
        private Transform Start1;
        private Transform Finish1;

        public GameObject Truck;
        private float speed = 1f;

        private bool backingUp = false;


        [Header("Player")] public GameObject player;



        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

            //Find positions
            Start1 = GameObject.Find("Start").transform;
            Finish1 = GameObject.Find("Finish").transform;


        }

        private void Update()
        {
            if (backingUp)
            {
                Truck.transform.position = Vector3.MoveTowards(Truck.transform.position, Finish1.transform.position, speed * Time.deltaTime);
            }
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

        public void On_BaselineS8_1_Start()
        {

        }

        public void On_BaselineS8_1_Finish()
        {

        }

        public void On_BaselineS8_2_Start()
        {
            print("On_BaselineS8_2_Start");
            // A truck backs up to carry the mud and leaves, and the water leaks along the path the truck goes.Repeat this multiple times, so the water stain on the ground grows bigger and it is approaching the wire on the ground.
            // get incident
            var incident = GetIncident(2);
            
            backingUp = true;

        }

        public void On_BaselineS8_2_Finish()
        {
            // A truck backs up to carry the mud and leaves, and the water leaks along the path the truck goes.Repeat this multiple times, so the water stain on the ground grows bigger and it is approaching the wire on the ground.
        }

        public void On_BaselineS8_3_Start()
        {
            print("On_BaselineS8_3_Start");
            // Repeat every time when participants are passing the road.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);
        }

        public void On_BaselineS8_3_Finish()
        {
            // Repeat every time when participants are passing the road.

        }


        public void On_BaselineS8_4_Start()
        {
            print("On_BaselineS8_4_Start");
            // SAGAT query
        }

        public void On_BaselineS8_4_Finish()
        {
            // SAGAT query

        }

        #endregion

    }
}