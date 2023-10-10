using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using UnityTimer;
using VRC2.Animations.CraneTruck;
using VRC2.Extention;
using Timer = UnityTimer.Timer;

namespace VRC2.Scenarios.ScenarioFactory
{
    internal enum CraneTruckStage
    {
        Stop = 0,
        Forward = 1,
        Backward = 2,
        Dropoff = 3,
    }

    public class BaselineS5 : Scenario
    {
        [Header("Gameobjects")] public GameObject craneTruck;
        public GameObject cargo;
        public Animator anim;

        [Space(30)] [Header("Recording/Replay")]
        public CraneTruckInputRecording recording;

        public CraneTruckInputReplay replay;

        public Transform startPoint;
        public Transform dropoffPoint;

        private bool isDroppingoff = false;

        // when to stop truck
        private float distanceThreshold = 2.0f;


        private Timer _timer;

        // current stage 
        private CraneTruckStage _stage;

        #region Transform backup/restore

        private BackupableTransform truckTransform;
        private BackupableTransform cargoTransform;


        #endregion

        private void Start()
        {
            base.Start();

            _stage = CraneTruckStage.Stop;

            truckTransform = BackupableTransform.Clone(craneTruck);
            cargoTransform = BackupableTransform.Clone(cargo);
        }

        private void Update()
        {
            switch (_stage)
            {
                case CraneTruckStage.Stop:
                    replay.Stop();
                    if (ReachedDestination(dropoffPoint.position) && recording.TruckStopped)
                    {
                        isDroppingoff = false;
                        // drop off
                        _stage = CraneTruckStage.Dropoff;
                    }

                    break;
                case CraneTruckStage.Forward:
                    replay.Forward(true);

                    if (ReachedDestination(startPoint.position))
                    {
                        _stage = CraneTruckStage.Stop;
                    }

                    break;
                case CraneTruckStage.Backward:
                    if (ReachedDestination(dropoffPoint.position))
                    {
                        _stage = CraneTruckStage.Stop;
                    }

                    break;
                case CraneTruckStage.Dropoff:
                    if (!isDroppingoff)
                    {
                        isDroppingoff = true;
                        replay.Dropoff();
                    }
                    else
                    {
                        if (replay.FinishDropoff())
                        {
                            _stage = CraneTruckStage.Forward;
                        }
                    }

                    break;
            }
        }

        void ResetTransforms()
        {
            truckTransform.Restore(ref craneTruck);
            cargoTransform.Restore(ref cargo);
        }

        void StartTimer(int second, Action oncomplete)
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(second, oncomplete, isLooped: false, useRealTime: true);
        }

        #region craneTruck control

        bool ReachedDestination(Vector3 des)
        {
            var t = craneTruck.transform.position;

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

        #endregion



        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S5");
        }

        public void On_BaselineS5_1_Start()
        {

        }

        public void On_BaselineS5_1_Finish()
        {

        }

        public void On_BaselineS5_2_Start()
        {
            print("On_BaselineS5_2_Start");
            // A crane truck loaded with windows parks next to the working zone, and is going to unload the windows next to participants.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;

            _stage = CraneTruckStage.Backward;
            replay.Backward(true);
        }

        public void On_BaselineS5_2_Finish()
        {
            // A crane truck loaded with windows parks next to the working zone, and is going to unload the windows next to participants.
        }

        public void On_BaselineS5_3_Start()
        {
            print("On_BaselineS5_3_Start");
            // The unload finishes and the crane truck leaves.
            // get incident
            var incident = GetIncident(3);

            // it already automatically moves forward
            // _stage = CraneTruckStage.Forward;
        }

        public void On_BaselineS5_3_Finish()
        {
            // The unload finishes and the crane truck leaves.

        }

        public void On_BaselineS5_4_Start()
        {
            print("On_BaselineS5_4_Start");
            // Another crane truck loaded with heavier windows parks and is going to unload the windows. This time the crane tilts a little bit.

            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            ResetTransforms();
            _stage = CraneTruckStage.Backward;
            replay.Backward(true);
        }

        public void On_BaselineS5_4_Finish()
        {
            // Another crane truck loaded with heavier windows parks and is going to unload the windows. This time the crane tilts a little bit.

        }

        public void On_BaselineS5_5_Start()
        {
            print("On_BaselineS5_5_Start");
            // The unload finishes and the crane truck leaves.
            // get incident
            var incident = GetIncident(5);

            // _stage = CraneTruckStage.Forward;
        }

        public void On_BaselineS5_5_Finish()
        {
            // The unload finishes and the crane truck leaves.

        }

        public void On_BaselineS5_6_Start()
        {
            print("On_BaselineS5_6_Start");
            // Another crane truck loaded with even heavier windows parks and is going to unload the windows. This time the crane is about to overturn.

            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;
            print(warning);

            ResetTransforms();
            _stage = CraneTruckStage.Backward;
            replay.Backward(true);
        }

        public void On_BaselineS5_6_Finish()
        {
            // Another crane truck loaded with even heavier windows parks and is going to unload the windows. This time the crane is about to overturn.

        }

        public void On_BaselineS5_7_Start()
        {
            print("On_BaselineS5_7_Start");

            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS5_7_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}