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
        public GameObject backHoe;
        public GameObject rocks;
        public GameObject dumpster;
        [Header("BackhoeController")] public Transform BackhoeController2;
        public Transform BackhoeController3;

        [Header("Dumpster")] public GameObject dumpster2;
        public GameObject dumpster3;


        [Header("Rock")] public GameObject rock2;
        public GameObject rock3;

        // public GameObject excavator;

        // public GameObject destination;

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

        private GameObject activeSetting;

        private BackhoeController _backhoeController;

        private WSMVehicleController _vehicleController;
        private void Start()
        {
            base.Start();

            // destinationPos = destination.transform.position;

            _backhoeController = backHoe.GetComponent<BackhoeController>();
            _vehicleController = backHoe.GetComponent<WSMVehicleController>();
        }

        private void Update()
        {

            if (lowerStabilizers)
            {
                _backhoeController.MoveStabilizerLegs(-1);
            }

            if (moving)
            {
                StartVehicle();
                MoveBackward();
            }
            else
            {
                StopVehicle();
            }

            if (extendBoom)
            {
                ExtendBoom();
            }

            if (extendArm)
            {
                ExtendArm();
            }

            if (extendRearBucket)
            {
                ExtendRearBucket();
            }

            if (retractBoom)
            {
                RetractBoom();
            }

            if (retractArm)
            {
                RetractArm();
            }

            if (retractRearBucket)
            {
                RetractRearBucket();
            }

            if (rotateBoom)
            {
                RotateBoom();
            }

            if (centerBoom)
            {
                CenterBoom();
            }

        }

        void UpdateTransforms(Transform bc, GameObject rk, GameObject ds)
        {
            // backHoe.SetActive(false);
            backHoe.transform.position = bc.position;
            backHoe.transform.rotation = bc.rotation;

            // backHoe.SetActive(true);

            rk.SetActive(true);
            ds.SetActive(true);
        }

        void EnableSetting(bool two, bool three)
        {
            rocks.SetActive(false);
            dumpster.SetActive(false);

            if (two)
            {
                UpdateTransforms(BackhoeController2, rock2, dumpster2);
                rock3.SetActive(false);
                dumpster3.SetActive(false);
            }
            else if (three)
            {
                UpdateTransforms(BackhoeController3, rock3, dumpster3);
                rock2.SetActive(false);
                dumpster2.SetActive(false);
            }

            // reset
            extendBoom = false;
            extendArm = false;
            extendRearBucket = false;
            retractBoom = false;
            retractArm = false;
            retractRearBucket = false;
            rotateBoom = false;
            centerBoom = false;
            lowerStabilizers = false;
            moving = false;


            _backhoeController.ResetBackhoe();
        }

        IEnumerator ExcavatorDig()
        {
            print("StartedCoroutine");
            lowerStabilizers = true;
            yield return new WaitForSeconds(2f);
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
            var e = _backhoeController.gameObject.transform.position;
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

            StopAllCoroutines();

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

            StopAllCoroutines();
            EnableSetting(true, false);

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

            StopAllCoroutines();

            EnableSetting(false, true);

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