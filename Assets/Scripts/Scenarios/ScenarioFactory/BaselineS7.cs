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

        [Header("BackhoeController")] public Transform BackhoeController1;
        public Transform BackhoeController2;
        public Transform BackhoeController3;

        [Header("Dumpster")] public GameObject dumpster1;
        public GameObject dumpster2;
        public GameObject dumpster3;


        [Header("Rock")] public GameObject rock1;
        public GameObject rock2;
        public GameObject rock3;

        [Header("Normal")] public Transform normalBackhoe;
        public GameObject normalDumpster;
        public GameObject normalRock;

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


        private void Start()
        {
            base.Start();

            // destinationPos = destination.transform.position;
        }

        private void Update()
        {
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

        void EnableSetting(int idx)
        {
        }

        IEnumerator ExcavatorDig()
        {
            yield break;
        }

        #region Driving Control

        bool ReachDestination()
        {
            return false;
        }


        void StopVehicle()
        {
        }

        void StartVehicle()
        {
        }

        void MoveBackward()
        {

            if (ReachDestination())
            {
                print("Truck reach destination");
                moving = false;
                return;
            }
        }

        #endregion

        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S7");

            StopAllCoroutines();

            EnableSetting(0);

            StartCoroutine(ExcavatorDig());
        }

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

            EnableSetting(1);

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
            EnableSetting(2);

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

            EnableSetting(3);

            StartCoroutine(ExcavatorDig());
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