using Oculus.Platform.Models;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS7 : Scenario
    {
        public GameObject excavator;

        public GameObject destination;

        private BackhoeController _backhoeController;
        private WSMVehicleController _vehicleController;

        private Vector3 destinationPos;

        private bool extendBoom = false;
        private bool extendArm = false;
        private bool extendRearBucket = false;

        private bool retractBoom = false;
        private bool retractArm = false;
        private bool retractRearBucket = false;

        private bool rotateBoom = false;
        private bool centerBoom = false;

        private bool lowerStabilizers = false;

        private bool moving = false;

        private float distanceThreshold = 5.0f;

        private void Start()
        {
            base.Start();

            _backhoeController = excavator.GetComponent<BackhoeController>();

            destinationPos = destination.transform.position;

            _vehicleController = excavator.GetComponent<WSMVehicleController>();
        }

        private void Update()
        {

            if (lowerStabilizers) { _backhoeController.MoveStabilizerLegs(-1); }

            if (moving)
            {
                StartVehicle();
                MoveBackward();
            }
            else
            {
                StopVehicle();
            }

            if (extendBoom) { ExtendBoom(); }
            if (extendArm) { ExtendArm(); }
            if (extendRearBucket) { ExtendRearBucket(); }
            if (retractBoom) { RetractBoom(); }
            if (retractArm) { RetractArm(); }
            if (retractRearBucket) { RetractRearBucket(); }
            if (rotateBoom) { RotateBoom(); }
            if (centerBoom) { CenterBoom(); }

        }

        IEnumerator ExcavatorDig()
        {
            print("StartedCoroutine");
            lowerStabilizers = true;
            yield return new WaitForSeconds(3f);
            extendArm = true;
            yield return new WaitForSeconds(3f);
            extendArm = false;
            extendRearBucket = true;
            extendBoom = true;
            yield return new WaitForSeconds(2f);
            extendRearBucket = false;
            extendBoom = false;
            retractArm = true;
            yield return new WaitForSeconds(3f);
            retractArm = false;
            retractBoom = true;
            retractRearBucket = true;
            yield return new WaitForSeconds(2f);
            retractBoom = false;
            retractRearBucket = false;
            yield return new WaitForSeconds(2f);
            rotateBoom = true;
            yield return new WaitForSeconds(2f);
            rotateBoom = false;
            yield return new WaitForSeconds(2f);
            extendArm = true;
            yield return new WaitForSeconds(2f);
            extendArm = false;
            extendBoom = true;
            yield return new WaitForSeconds(1f);
            extendBoom = false;
            extendRearBucket = true;
            yield return new WaitForSeconds(3f);
            extendRearBucket = false;
            retractArm = true;
            retractBoom = true;
            retractRearBucket = true;
            yield return new WaitForSeconds(3f);
            retractArm = false;
            retractBoom = false;
            retractRearBucket = false;
            centerBoom = true;
            yield return new WaitForSeconds(2f);
            centerBoom = false;
            yield break;
        }

        #region Excavator Control

        void ExtendBoom()
        { 
            _backhoeController.MoveBoom(1);
        }

        void RetractBoom()
        {
            _backhoeController.MoveBoom(-1);
        }

        void ExtendArm()
        {
            _backhoeController.MoveArm(1);
        }

        void RetractArm()
        {
            _backhoeController.MoveArm(-1);
        }

        void ExtendRearBucket()
        {
            _backhoeController.MoveRearBucket(1);
        }

        void RetractRearBucket()
        {
            _backhoeController.MoveRearBucket(-1);
        }

        void RotateBoom()
        {
            _backhoeController.MoveSwingFrame(1);
        }

        void CenterBoom()
        {
            _backhoeController.MoveSwingFrame(-1);
        }


        #endregion

        #region Driving Control

        bool ReachDestination()
        {
            var d = destinationPos;

            // ignore y distance
            var e = excavator.transform.position;
            d.y = e.y;
            var distance = Vector3.Distance(e, d);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
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

        void MoveBackward()
        {
            _vehicleController.AccelerationInput = -1.0f;

            if (ReachDestination())
            {
                print("Truck reach destination");
                moving = false;
                return;
            }
        }

        #endregion

        #region Accident Events Callbacks

        public void On_BaselineS7_1_Start()
        {
            moving = true;
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

            StartCoroutine(ExcavatorDig());

        }

        public void On_BaselineS7_2_Finish()
        {
            // An excavator is digging next to the participants.
            StopAllCoroutines();
        }

        public void On_BaselineS7_3_Start()
        {
            print("On_BaselineS7_3_Start");
            // The excavator is digging into the working zone.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);

            StartCoroutine(ExcavatorDig());
        }

        public void On_BaselineS7_3_Finish()
        {
            // The excavator is digging into the working zone.
            StopAllCoroutines();
        }

        public void On_BaselineS7_4_Start()
        {
            print("On_BaselineS7_4_Start");
            // The excavator is digging more into the working zone.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            StartCoroutine(ExcavatorDig());
        }

        public void On_BaselineS7_4_Finish()
        {
            // The excavator is digging more into the working zone.
            StopAllCoroutines();
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