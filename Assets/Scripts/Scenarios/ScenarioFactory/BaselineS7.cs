using Oculus.Platform.Models;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using VRC2.Animations;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS7 : Scenario
    {
        public GameObject excav;

        internal enum ExcavatorStage
        {
            Stop = 0,
            Forward = 1,
            Backward = 2,
            Dig = 3,
            Left = 4,
            Right = 5,
        }

        [Space(30)]  [Header("Recording/Replay")]
        public ExcavatorInputRecording recording;
               

        public ExcavatorInputReplay replay;

        public Transform startPoint;
        public Transform digPoint;



        [Header("Rock")] public GameObject rock1;
        public GameObject rock2;
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

        private bool isDigging = false;

        private float distanceThreshold = 3.0f;

        private GameObject activeSetting;

        private ExcavatorStage _stage;

        


        private void Start()
        {
            base.Start();
            _stage = ExcavatorStage.Stop;
            // destinationPos = destination.transform.position;
        }

        private void Update()
        {
            switch (_stage)
            {
                case ExcavatorStage.Stop:
                    //replay.Stop();
                    if (ReachDestination(digPoint.position) && (recording.driveSpeed == 0))
                    {
                        isDigging = false;
                        // time to dig
                        _stage = ExcavatorStage.Dig;
                    }

                    break;
                case ExcavatorStage.Forward:
                    //replay.Forward(true);
                    if (ReachDestination(startPoint.position))
                    {
                        _stage = ExcavatorStage.Stop;
                    }

                    break;
                case ExcavatorStage.Backward:
                    if (ReachDestination(digPoint.position))
                    {
                        _stage = ExcavatorStage.Stop;
                    }

                    break;

                case ExcavatorStage.Dig:
                    if (!isDigging)
                    {
                        isDigging = true;
                        //replay.Dig();
                    }
                    else
                    {
                        if (replay.DigFinished())
                        {
                            _stage = ExcavatorStage.Forward;
                        }
                    }

                    break;
            }
            }
        

        void UpdateTransforms(Transform bc, GameObject rk, GameObject ds)
        {
            // backHoe.SetActive(false);
            excav.transform.position = bc.position;
            excav.transform.rotation = bc.rotation;

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

        bool ReachDestination(Vector3 des)
        {
            var t = excav.transform.position;

            // use the same y
            des.y = 0;
            t.y = 0;
            var distance = Vector3.Distance(t, des);

            if (distance < distanceThreshold)
            {
                return true;
            }

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

            if (ReachDestination(startPoint.position))
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