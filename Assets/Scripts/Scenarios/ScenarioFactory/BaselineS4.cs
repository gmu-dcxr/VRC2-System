using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS4 : Scenario
    {
        [Header("TruckPositions")] private Transform Start1;
        private Transform Finish1;
        private Transform Start2;
        private Transform Finish2;
        private Transform Start3;
        private Transform Finish3;

        public GameObject drone;
        private float speed = 6f;

        private bool backingUp1 = false;
        private bool backingUp2 = false;
        private bool backingUp3 = false;
        private bool movingForward1 = false;
        private bool movingForward2 = false;

        [Header("Player")] public GameObject player;



        private void Start()
        {
            base.Start();
            drone = GameObject.Find("_Drone [BumbleBee]");

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
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, Finish1.transform.position,
                    speed * Time.deltaTime);
            }

            if (movingForward1)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, Start1.transform.position,
                    speed * Time.deltaTime);
            }

            if (backingUp2)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, Finish2.transform.position,
                    speed * Time.deltaTime);
            }

            if (movingForward2)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, Start2.transform.position,
                    speed * Time.deltaTime);
            }

            if (backingUp3)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, Finish3.transform.position,
                    speed * Time.deltaTime);
            }
        }


        #region Accident Events Callbacks

        public void On_BaselineS4_1_Start()
        {

        }

        public void On_BaselineS4_1_Finish()
        {

        }

        public void On_BaselineS4_2_Start()
        {
            print("On_BaselineS4_2_Start");
            // A supervising drone is approaching.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            backingUp1 = true;
        }

        public void On_BaselineS4_2_Finish()
        {
            // A supervising drone is approaching.
        }

        public void On_BaselineS4_3_Start()
        {
            print("On_BaselineS4_3_Start");
            // The drone leaves.
            // get incident
            var incident = GetIncident(3);

            backingUp1 = false;
            movingForward1 = true;
        }

        public void On_BaselineS4_3_Finish()
        {
            // The drone leaves.

        }

        public void On_BaselineS4_4_Start()
        {
            print("On_BaselineS4_4_Start");
            // Another drone is approaching at a lower height.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            movingForward1 = false;
            drone.transform.position = new Vector3(Start2.transform.position.x, Start2.transform.position.y,
                Start2.transform.position.z);
            backingUp2 = true;
        }

        public void On_BaselineS4_4_Finish()
        {
            // Another drone is approaching at a lower height.
        }

        public void On_BaselineS4_5_Start()
        {
            print("On_BaselineS4_5_Start");
            // The drone leaves.
            // get incident
            var incident = GetIncident(5);


            backingUp2 = false;
            movingForward2 = true;
        }

        public void On_BaselineS4_5_Finish()
        {
            // The drone leaves.

        }

        public void On_BaselineS4_6_Start()
        {
            print("On_BaselineS4_6_Start");
            // Another drone is approaching at a height that is going to collide with participants.
            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;
            print(warning);

            movingForward2 = false;
            drone.transform.position = new Vector3(Start3.transform.position.x, Start3.transform.position.y,
                Start3.transform.position.z);
            backingUp3 = true;
        }

        public void On_BaselineS4_6_Finish()
        {
            // Another drone is approaching at a height that is going to collide with participants.
        }

        public void On_BaselineS4_7_Start()
        {
            print("On_BaselineS4_7_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS4_7_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}