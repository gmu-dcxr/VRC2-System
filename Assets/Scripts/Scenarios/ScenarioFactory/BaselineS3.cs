using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS3 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS3.yml";

        [Header("Accident Configure")] public GameObject pipes;

        [Header("TruckPositions")]
        private Transform Start1;
        private Transform Finish1;
        private Transform Start2;
        private Transform Finish2;
        private Transform Start3;
        private Transform Finish3;

        public GameObject truck;
        private float speed = 6f;

        private bool backingUp1 = false;
        private bool backingUp2 = false;
        private bool backingUp3 = false;
        private bool movingForward1 = false;
        private bool movingForward2 = false;

        [Header("Player")] public GameObject player;



        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

            pipes.SetActive(false);

            //Find positions
            Start1 = GameObject.Find("Start").transform;
            Finish1 = GameObject.Find("Finish").transform;
            Start2 = GameObject.Find("Start2").transform;
            Finish2 = GameObject.Find("Finish2").transform;
            Start3 = GameObject.Find("Start3").transform;
            Finish3 = GameObject.Find("Finish3").transform;
        }

        private void Update()
        {
            if (backingUp1)
            {
                truck.transform.position = Vector3.MoveTowards(truck.transform.position, Finish1.transform.position, speed * Time.deltaTime);
            }
            if (movingForward1)
            {
                truck.transform.position = Vector3.MoveTowards(truck.transform.position, Start1.transform.position, speed * Time.deltaTime);
            }
            if (backingUp2)
            {
                truck.transform.position = Vector3.MoveTowards(truck.transform.position, Finish2.transform.position, speed * Time.deltaTime);
            }
            if (movingForward2)
            {
                truck.transform.position = Vector3.MoveTowards(truck.transform.position, Start2.transform.position, speed * Time.deltaTime);
            }
            if (backingUp3)
            {
                truck.transform.position = Vector3.MoveTowards(truck.transform.position, Finish3.transform.position, speed * Time.deltaTime);
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

        public void On_BaselineS3_1_Start()
        {
        
        }

        public void On_BaselineS3_1_Finish()
        {

        }

        public void On_BaselineS3_2_Start()
        {
            print("On_BaselineS3_2_Start");
            // A truck is backing up nearby.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            backingUp1 = true;
        }

        public void On_BaselineS3_2_Finish()
        {
            // A truck is backing up nearby.
        }

        public void On_BaselineS3_3_Start()
        {
            print("On_BaselineS3_3_Start");
            // The loaded truck is passing nearby.
            // get incident
            var incident = GetIncident(3);

            backingUp1 = false;
            movingForward1 = true;
            pipes.SetActive(true);
        }

        public void On_BaselineS3_3_Finish()
        {
            // The loaded truck is passing nearby.
            
        }

        public void On_BaselineS3_4_Start()
        {
            print("On_BaselineS3_4_Start");
            // Another truck is backing up nearby, and it is closer to the participants.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            movingForward1 = false;
            truck.transform.position = new Vector3(Start2.transform.position.x, Start2.transform.position.y, Start2.transform.position.z);
            pipes.SetActive(false);
            backingUp2 = true;
        }

        public void On_BaselineS3_4_Finish()
        {
            // Another truck is backing up nearby, and it is closer to the participants.
        }

        public void On_BaselineS3_5_Start()
        {
            print("On_BaselineS3_5_Start");
            // The loaded truck is passing nearby.
            // get incident
            var incident = GetIncident(5);

            backingUp2 = false;
            movingForward2 = true;
            pipes.SetActive(true);
        }

        public void On_BaselineS3_5_Finish()
        {
            // The loaded truck is passing nearby.
            
        }

        public void On_BaselineS3_6_Start()
        {
            print("On_BaselineS3_6_Start");
            // Another truck is backing up nearby, and it is going to collide with the participants.
            // get incident
            var incident = GetIncident(6);

            movingForward2 = false;
            truck.transform.position = new Vector3(Start3.transform.position.x, Start3.transform.position.y, Start3.transform.position.z);
            pipes.SetActive(false);
            backingUp3 = true;
        }

        public void On_BaselineS3_6_Finish()
        {
            // Another truck is backing up nearby, and it is going to collide with the participants.
        }

        public void On_BaselineS3_7_Start()
        {
            print("On_BaselineS3_7_Start");
            // SAGAT query
        }

        public void On_BaselineS3_7_Finish()
        {
            // SAGAT query
          
        }

        #endregion

    }
}