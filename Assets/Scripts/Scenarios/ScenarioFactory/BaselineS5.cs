using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using UnityTimer;
using Timer = UnityTimer.Timer;

namespace VRC2.Scenarios.ScenarioFactory
{
    public enum WorkStage
    {
        Stop = 0,
        UpLift = 1,
        Forward = 2,
        DownLift = 3,
        Back = 4,
    }

    public class BaselineS5 : Scenario
    {
        public GameObject craneTruck;
        
        [Header("Loads")]
        public GameObject normalGood;
        public GameObject heavierGood;
        public GameObject heaviestGood;

        [Header("Positions")] public GameObject destination;

        [Header("Tilt")] public Transform titleTransform;

        public Transform overturnTransfrom;
        
        private Vector3 destinationPos;
        
        private Vector3 startPos;
        private Quaternion startRotation;

        private WSMVehicleController _vehicleController;
        private ForkliftController _forkliftController;

        private float speed = 6f;
        private float rotationSpeed = 0f;

        private float distanceThreshold = 4.0f;

        private GameObject player;

        private Timer _timer;

        private WorkStage _stage;

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


        private void Start()
        {
            base.Start();

            player = localPlayer;

            _stage = WorkStage.Stop;

            moving = false;

            EnableGoods(true, false, false);


            //Find positions
            startPos = craneTruck.transform.position;
            startRotation = craneTruck.transform.rotation;
            destinationPos = destination.transform.position;

            _vehicleController = craneTruck.GetComponent<WSMVehicleController>();
            _forkliftController = craneTruck.GetComponent<ForkliftController>();
        }

        private void Update()
        {
            if (!moving) return;

            switch (_stage)
            {
                case WorkStage.Stop:
                    StopVehicle();
                    break;
                case WorkStage.UpLift:
                    if (ReachedLiftHeight(true))
                    {
                        _stage = WorkStage.Forward;
                    }
                    else
                    {
                        LiftLoad(true);
                    }

                    break;
                case WorkStage.Forward:
                    if (ReachedDestination(true))
                    {
                        // stop
                        _stage = WorkStage.Stop;
                    }
                    else
                    {
                        MoveForward(true);
                    }

                    break;
                case WorkStage.Back:

                    if (ReachedDestination(false))
                    {
                        // stop
                        _stage = WorkStage.Stop;
                    }
                    else
                    {
                        MoveForward(false);
                    }

                    break;
                case WorkStage.DownLift:
                    if (ReachedLiftHeight(false))
                    {
                        // fix a bug that sometimes the load can not be unloaded.

                        load.GetComponent<Rigidbody>().useGravity = false;
                        load.transform.Translate(0f, 0.1f, 0f, Space.Self);
                        StartTimer(1, () => { load.GetComponent<Rigidbody>().useGravity = true; });

                        _stage = WorkStage.Back;
                    }
                    else
                    {
                        LiftLoad(false);
                    }

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
            _forkliftController.MoveForksVertically(value);
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

        bool ReachedDestination(bool _forward)
        {
            var d = destinationPos; // forward
            if (!_forward)
            {
                d = startPos; // back
            }

            // ignore y distance
            var t = craneTruck.transform.position;
            d.y = t.y;
            var distance = Vector3.Distance(t, d);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        bool ReachedLiftHeight(bool up)
        {
            if (up)
            {
                if (_forkliftController.ForksVertical > liftHeightThreshold)
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

                if (_forkliftController.ForksVertical <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

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
            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }

            _vehicleController.AccelerationInput = _acceleration;
        }

        #endregion



        #region Accident Events Callbacks

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
            
            ResetTruck();

            EnableGoods(true, false, false);

            _stage = WorkStage.UpLift;
            moving = true;
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

            _stage = WorkStage.DownLift;
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

            _stage = WorkStage.UpLift;
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

            _stage = WorkStage.DownLift;
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

            _stage = WorkStage.UpLift;
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