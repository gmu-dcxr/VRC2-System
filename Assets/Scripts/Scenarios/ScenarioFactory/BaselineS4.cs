using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS4 : Scenario
    {
        public GameObject drone;

        private float speed = 6f;

        private GameObject player;

        private bool approaching = false;

        private Vector3 droneInitPosition;

        [Header("Height Offset")]
        [Tooltip("Off the ground")]
        public float lowerHeight = 5.0f;

        [Tooltip("Off the ground")]
        public float collidingHeight = 0.5f;

        private Vector3 destination;

        private bool moving = false;



        private void Start()
        {
            base.Start();

            droneInitPosition = drone.transform.position;
            
            player = localPlayer;
            approaching = false;
        }

        private void Update()
        {
            if(!moving) return;
            
            if (approaching)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, destination,
                    speed * Time.deltaTime);
            }
            else
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, droneInitPosition,
                    speed * Time.deltaTime);
            }
        }

        void UpdateDestination(float heightoffset)
        {
            var pos = player.transform.position;
            pos.y += heightoffset;

            destination = pos;
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

            var offset = drone.transform.position.y - player.transform.position.y;
            UpdateDestination(offset);

            moving = true;
            approaching = true;
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

            moving = true;
            approaching = false;
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

            UpdateDestination(lowerHeight);
            
            moving = true;
            approaching = true;
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

            moving = true;
            approaching = false;
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

            UpdateDestination(collidingHeight);
            moving = true;
            approaching = true;
        }

        public void On_BaselineS4_6_Finish()
        {
            // Another drone is approaching at a height that is going to collide with participants.
        }

        public void On_BaselineS4_7_Start()
        {
            // make drone runaway
            moving = true;
            approaching = false;
            
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