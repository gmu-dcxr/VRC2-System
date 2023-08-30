using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VRC2.Scenarios.ScenarioFactory;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;

namespace VRC2.Scenarios
{
    public class CustomForkLiftController : MonoBehaviour
    {
        internal enum WorkStage
        {
            Stop = 0,
            Forward = 1,
            UpLift = 2,
            Back = 3,
            DownLift = 4,
        }

        public GameObject forklift;
        public GameObject good;
        public Transform destination;

        private WSMVehicleController _vehicleController;
        private ForkliftController _forkliftController;

        private Vector3 destinationPos;

        private Vector3 startPos;
        private Quaternion startRotation;

        private Vector3 goodStartPos;
        private Quaternion goodStartRotation;

        private WorkStage _stage;
        private float liftHeightThreshold = 0.5f;

        private bool moving = false;
        private float distanceThreshold = 3.0f;


        private void Start()
        {
            _stage = WorkStage.Stop;

            moving = false;

            //Find positions
            startPos = forklift.transform.position;
            startRotation = forklift.transform.rotation;
            destinationPos = destination.transform.position;

            goodStartPos = good.transform.position;
            goodStartRotation = good.transform.rotation;

            _vehicleController = forklift.GetComponent<WSMVehicleController>();
            _forkliftController = forklift.GetComponent<ForkliftController>();
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

                    break;
                case WorkStage.DownLift:
                    break;
            }
        }
        
        void StopVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;

            print(_vehicleController.CurrentSpeed.ToString("f5"));
            if (_vehicleController.CurrentSpeed < 0.1)
            {
                // change stage
                ResetStatus();
            }
        }

        private void ResetStatus()
        {
            _forkliftController.forks.ResetToMin();
            _forkliftController.ForksVertical = 0;
            
            forklift.transform.position = startPos;
            forklift.transform.rotation = startRotation;

            good.transform.position = goodStartPos;
            good.transform.rotation = goodStartRotation;

            _stage = WorkStage.UpLift;
        }

        void LiftLoad(bool up)
        {
            var value = -1;
            if (up) value = 1;
            // make it still
            _forkliftController.MoveForksVertically(value);
        }

        bool ReachedDestination(bool _forward)
        {
            var d = destinationPos; // forward
            if (!_forward)
            {
                d = startPos; // back
            }

            // ignore y distance
            var t = forklift.transform.position;
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

        void MoveForward(bool forward)
        {
            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
            
            _vehicleController.AccelerationInput = _acceleration;
        }

        public void Animate()
        {
            moving = true;

            _stage = WorkStage.UpLift;
        }
    }
}