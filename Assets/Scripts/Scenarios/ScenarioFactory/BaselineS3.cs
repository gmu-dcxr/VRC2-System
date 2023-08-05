using System;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS3 : Scenario
    {
        [Header("Truck")] public GameObject truck;
        public GameObject truckLoad;

        // truck destination
        [Header("Markers")] public GameObject destination;

        public GameObject closerStart;
        public GameObject collisionStart;

        private Vector3 startPos;
        private Vector3 destinationPos;
        private GameObject player;

        private float distanceThreshold = 5.0f;

        private WSMVehicleController _vehicleController;

        private bool moving = false;
        private bool back = false;

        private void Start()
        {
            base.Start();

            startPos = truck.transform.position;
            destinationPos = destination.transform.position;

            _vehicleController = truck.GetComponent<WSMVehicleController>();
        }

        private void Update()
        {
            if (!moving)
            {
                StopVehicle();
            }
            else
            {
                MoveForward(!back);
            }
        }

        #region Truck Control

        bool ReachDestination(bool forward)
        {
            var d = destinationPos; // backward
            if (forward)
            {
                d = startPos;
            }

            // ignore y distance
            var t = truck.transform.position;
            d.y = t.y;
            var distance = Vector3.Distance(t, d);

            print(distance);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        void ShowLoad(bool flag)
        {
            truckLoad.SetActive(flag);
        }

        void StopVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;
        }

        void StartVehicle()
        {
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
        }

        void MoveForward(bool forward)
        {
            if (ReachDestination(forward))
            {
                print("Truck reach destination");
                moving = false;
                return;
            }

            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }

            _vehicleController.AccelerationInput = _acceleration;
        }

        #endregion

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

            ShowLoad(false);

            moving = true;
            back = true;

            StartVehicle();
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

            ShowLoad(true);

            moving = true;
            back = false;

            StartVehicle();
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

            // update truck pos & rot
            truck.transform.position = closerStart.transform.position;
            truck.transform.rotation = closerStart.transform.rotation;

            // overwrite start point
            startPos = closerStart.transform.position;

            ShowLoad(false);

            moving = true;
            back = true;

            StartVehicle();
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

            ShowLoad(true);

            moving = true;
            back = false;

            StartVehicle();
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

            // update truck pos & rot
            truck.transform.position = collisionStart.transform.position;
            truck.transform.rotation = collisionStart.transform.rotation;

            // overwrite start point
            startPos = collisionStart.transform.position;

            ShowLoad(false);

            moving = true;
            back = true;

            StartVehicle();
        }

        public void On_BaselineS3_6_Finish()
        {
            // Another truck is backing up nearby, and it is going to collide with the participants.
        }

        public void On_BaselineS3_7_Start()
        {
            print("On_BaselineS3_7_Start");

            // hide truck
            truck.SetActive(false);

            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS3_7_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}