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

        private BackhoeController _backhoeController;

        private bool extendBoom = false;
        private bool extendArm = false;
        private bool extendRearBucket = false;

        private bool retractBoom = false;
        private bool retractArm = false;
        private bool retractRearBucket = false;

        private bool rotateBoom = false;
        private bool centerBoom = false;

        private void Start()
        {
            base.Start();

            _backhoeController = excavator.GetComponent<BackhoeController>(); 
        }

        private void Update()
        {
            _backhoeController.MoveStabilizerLegs(-1);

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