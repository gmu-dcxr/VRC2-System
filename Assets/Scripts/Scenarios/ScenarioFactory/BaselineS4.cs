using System;
using PathCreation.Examples;
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

        [Header("Height Offset")] [Tooltip("Off the ground")]
        private float normalHeight = 7f;

        private float lowerHeight = 6f;
        private float collidingHeight = 5f;

        private float hoveringThreshold = 0.1f;

        private Vector3 destination;

        private bool moving = false;

        private PathFollower _pathFollower;
        
        // resolve the conflicts between of BaselineS2 and BaselineS4
        [HideInInspector] public bool controllingDrone = true;


        private void Start()
        {
            base.Start();

            droneInitPosition = drone.transform.position;
            // use center eye to represent the player
            player = CenterEyeTransform.gameObject;
            approaching = false;

            _pathFollower = drone.GetComponent<PathFollower>();

            // disable at the beginning
            _pathFollower.enabled = false;
        }

        private void Update()
        {
            if(!controllingDrone) return;
            
            if (!moving) return;

            if (approaching)
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, destination,
                    speed * Time.deltaTime);

                if (Vector3.Distance(drone.transform.position, destination) < hoveringThreshold)
                {
                    // make it hovering
                    moving = false;
                    _pathFollower.enabled = true;
                    // hack height
                    _pathFollower.currentHeight = drone.transform.position.y;
                    _pathFollower.customizedHeight = true;
                }
            }
            else
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, droneInitPosition,
                    speed * Time.deltaTime);
                if (Vector3.Distance(drone.transform.position, droneInitPosition) < hoveringThreshold)
                {
                    // make it invisible
                    drone.SetActive(false);
                }
            }
        }

        void UpdateDestination(float heightoffset)
        {
            // enable it
            drone.SetActive(true);
            
            // calculate height
            var pos = player.transform.position;
            pos.y += heightoffset;
            
            // update drone height
            var dronePos = drone.transform.position;
            dronePos.y = pos.y;

            drone.transform.position = dronePos;
            
            // set destination
            destination = pos;
        }

        void ResetPathFollower()
        {
            // disable it
            _pathFollower.enabled = false;
            _pathFollower.customizedHeight = false;
        }


        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S4");

            _pathFollower.enabled = true;
        }

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

            ResetPathFollower();

            // var offset = drone.transform.position.y - player.transform.position.y;
            UpdateDestination(normalHeight);

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

            ResetPathFollower();

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

            ResetPathFollower();

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

            ResetPathFollower();

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

            ResetPathFollower();

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

            ResetPathFollower();

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