using System;
using UnityEngine;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using UnityTimer;
using VRC2.Animations;

namespace VRC2.Scenarios.ScenarioFactory
{
    public enum TruckStatus
    {
        Stop = 0,
        Forward = 1,
        Backward = 2,
        LeftTurn = 3,
        LeftTurnForward = 4,
        RightTurn = 5,
        RightTurnBack = 6,
        Waiting = 7,
    }

    public class BaselineS3_OLD : Scenario
    {
        [Header("Truck")] public GameObject truck;
        // public GameObject truckLoad;

        private Vector3 destinationPos;
        private GameObject player;

        [Space(30)] [Header("Recording/Replay")]
        public TruckInputRecording recording;

        public TruckInputReplay replay;

        [Space(30)] [Header("Markers")] public Transform backStart;
        public Transform turnLeft;
        public Transform turnLeftDone;
        public Transform turnRight;
        public Transform turnRightDone;
        public Transform End1;
        public Transform End2;
        public Transform End3;

        [Space(30)] [Header("Settings")] public float distanceThreshold = 0.5f;
        public bool autoCorrection = true;


        private TruckStatus _status;

        private Transform backDst;

        private void Start()
        {
            base.Start();

            _status = TruckStatus.Stop;
        }

        private void Update()
        {
            switch (_status)
            {
                case TruckStatus.Stop:
                    replay.StopAll();
                    replay.Brake();
                    break;
                case TruckStatus.Forward:
                    if (ReachDestination())
                    {
                        // stop it
                        replay.StopAll();
                        recording.ZeroSpeed();

                        // start turn left
                        _status = TruckStatus.LeftTurn;

                        // update destination
                        destinationPos = turnLeftDone.position;
                    }
                    else
                    {
                        replay.Forward(true);
                    }

                    break;

                case TruckStatus.Backward:
                    if (ReachDestination())
                    {
                        // stop it
                        replay.StopAll();

                        recording.ZeroSpeed();

                        // start turn right
                        _status = TruckStatus.RightTurn;

                        // update destination
                        destinationPos = turnRightDone.position;
                    }
                    else
                    {
                        replay.Backward(true);
                    }

                    break;

                case TruckStatus.LeftTurn:
                    if (replay.TurnLeftDone())
                    {
                        replay.StopAll();

                        _status = TruckStatus.LeftTurnForward;
                        recording.ZeroSpeed();

                        if (autoCorrection)
                        {
                            // force update position
                            truck.transform.position = turnLeftDone.position;
                            truck.transform.rotation = turnLeftDone.rotation;
                        }

                        destinationPos = backStart.position;
                    }
                    else
                    {
                        replay.TurnLeft();
                    }

                    break;

                case TruckStatus.RightTurn:
                    if (replay.TurnRightDone())
                    {
                        replay.StopAll();

                        _status = TruckStatus.RightTurnBack;
                        recording.ZeroSpeed();

                        if (autoCorrection)
                        {
                            // force update position
                            truck.transform.position = turnRightDone.position;
                            truck.transform.rotation = turnRightDone.rotation;
                        }

                        destinationPos = backDst.position;
                    }
                    else
                    {
                        replay.TurnRight();
                    }

                    break;

                case TruckStatus.RightTurnBack:
                    if (ReachDestination())
                    {
                        recording.ZeroSpeed();
                        _status = TruckStatus.Stop;
                    }
                    else
                    {
                        replay.Backward(true);
                    }

                    break;

                case TruckStatus.LeftTurnForward:
                    if (ReachDestination())
                    {
                        recording.ZeroSpeed();
                        _status = TruckStatus.Stop;
                    }
                    else
                    {
                        replay.Forward(true);
                    }

                    break;
            }
        }

        #region Truck Control

        bool ReachDestination()
        {
            // ignore y distance
            var t = truck.transform.position;
            t.y = destinationPos.y;
            var distance = Vector3.Distance(t, destinationPos);

            print(distance);

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
            print("Start Normal Incident Baseline S3");
        }

        public void On_BaselineS3_1_Start()
        {

        }

        public void On_BaselineS3_1_Finish()
        {

        }

        void ResetTruck()
        {
            recording.ZeroSpeed();
            truck.transform.position = backStart.position;
            truck.transform.rotation = backStart.rotation;
        }

        public void On_BaselineS3_2_Start()
        {
            print("On_BaselineS3_2_Start");
            // A truck is backing up nearby.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            replay.RewindAll();
            ResetTruck();
            destinationPos = turnRight.position;
            backDst = End1;
            _status = TruckStatus.Backward;
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

            destinationPos = turnLeft.position;
            _status = TruckStatus.Forward;
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

            replay.RewindAll();
            ResetTruck();
            destinationPos = turnRight.position;
            backDst = End2;
            _status = TruckStatus.Backward;
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

            destinationPos = turnLeft.position;
            _status = TruckStatus.Forward;
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

            replay.RewindAll();
            ResetTruck();
            destinationPos = turnRight.position;
            backDst = End3;
            _status = TruckStatus.Backward;
        }

        public void On_BaselineS3_6_Finish()
        {
            // Another truck is backing up nearby, and it is going to collide with the participants.
        }

        public void On_BaselineS3_7_Start()
        {
            print("On_BaselineS3_7_Start");

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