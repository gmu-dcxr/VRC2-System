using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using UnityTimer;
using VRC2.Animations.CraneTruck;
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
        [Header("Loads")] public GameObject normalGood;
        public GameObject heavierGood;
        public GameObject heaviestGood;

        [Header("Positions")] public GameObject destination;

        [Header("Tilt")] public Transform titleTransform;

        public Transform overturnTransfrom;

        private Vector3 destinationPos;

        private Vector3 startPos;
        private Quaternion startRotation;

        private ControllerTruck _vehicleController;

        private float speed = 6f;
        private float rotationSpeed = 0f;

        private float distanceThreshold = 2.0f;

        private GameObject player;

        private Timer _timer;

        private CraneTruckStage _stage;

        private float liftHeightThreshold = 0.5f;

        private bool moving = false;

        private GameObject load
        {
            get
            {
                if (normalGood.activeSelf) return normalGood;
                if (heavierGood.activeSelf) return heavierGood;
                if (heaviestGood.activeSelf) return heaviestGood;
                return null;
            }
        }

        #region New Version

        [Space(30)] [Header("Recording/Replay")]
        public CraneTruckInputRecording recording;

        public CraneTruckInputReplay replay;

        public Transform startPoint;
        public Transform dropoffPoint;

        private GameObject craneTruck
        {
            get => recording.CraneTruck;
        }


        #endregion


        private void Start()
        {
            base.Start();

            player = localPlayer;

            _stage = CraneTruckStage.Stop;

            moving = false;

            EnableGoods(true, false, false);


            //Find positions
            startPos = craneTruck.transform.position;
            startRotation = craneTruck.transform.rotation;
            destinationPos = destination.transform.position;

            _vehicleController = craneTruck.GetComponent<ControllerTruck>();
        }

        private void Update()
        {
            switch (_stage)
            {
                case CraneTruckStage.Stop:
                    replay.Stop();
                    break;
                case CraneTruckStage.Forward:
                    break;
                case CraneTruckStage.Backward:
                    if (ReachedDestination())
                    {
                        _stage = CraneTruckStage.Stop;
                    }

                    break;
                case CraneTruckStage.Dropoff:
                    break;
            }
        }

        void ResetTruck()
        {
            craneTruck.transform.position = startPos;
            craneTruck.transform.rotation = startRotation;
        }

        void EnableGoods(bool normal, bool heavier, bool heaviest)
        {
            normalGood.SetActive(normal);
            heavierGood.SetActive(heavier);
            heaviestGood.SetActive(heaviest);
        }

        void LiftLoad(bool up)
        {
            var value = -1;
            if (up) value = 1;
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

        bool ReachedDestination()
        {
            var d = dropoffPoint.position;
            var t = craneTruck.transform.position;
            
            // use the same y
            d.y = 0;
            t.y = 0;
            var distance = Vector3.Distance(t, d);
            
            print(distance);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        /*
        bool ReachedLiftHeight(bool up)
        {
            if (up)
            {
                if (_vehicleController. > liftHeightThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {

                if (_vehicleController. <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        */


        void StopVehicle()
        {
            _vehicleController.Brake();
        }

        void StartVehicle()
        {

        }

        void MoveForward(bool forward)
        {
            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }

            _vehicleController.speedEngine = _acceleration;
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
            print(warning);

            _stage = CraneTruckStage.Backward;

            replay.Backward(true);
            // ResetTruck();

            // EnableGoods(true, false, false);

            // _stage = CraneTruckStage.UpLift;
            // moving = true;
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

            _stage = CraneTruckStage.Forward;
            moving = true;
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

            ResetTruck();

            craneTruck.transform.rotation = titleTransform.rotation;

            EnableGoods(false, true, false);

            _stage = CraneTruckStage.Forward;
            moving = true;
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

            _stage = CraneTruckStage.Forward;
            moving = true;
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

            ResetTruck();

            craneTruck.transform.rotation = overturnTransfrom.rotation;

            EnableGoods(false, false, true);

            _stage = CraneTruckStage.Backward;
            moving = true;
        }

        public void On_BaselineS5_6_Finish()
        {
            // Another crane truck loaded with even heavier windows parks and is going to unload the windows. This time the crane is about to overturn.

        }

        public void On_BaselineS5_7_Start()
        {
            print("On_BaselineS5_7_Start");

            moving = false;

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