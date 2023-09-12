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
            Right = 5,
            Left = 6,
        }

        public GameObject forklift;
        public GameObject good;
        public Transform destination;
        public Transform turn;
        public Transform turnEnd;

        private WSMVehicleController _vehicleController;
        private ForkliftController _forkliftController;

        private Vector3 destinationPos;

        private Vector3 turnPos;

        private Vector3 turnEndPos;

        private Vector3 startPos;
        private Quaternion startRotation;

        private Vector3 goodStartPos;
        private Quaternion goodStartRotation;

        private WorkStage _stage;
        private float liftHeightThreshold = 0.65f;

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
            turnPos = turn.transform.position;
            turnEndPos = turnEnd.transform.position;

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
                        _stage = WorkStage.Back;
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
                    if (ReachedTurn(true))
                    {
                        _stage = WorkStage.Left;
                    }
                    else
                    {
                        MoveForward(false);
                    }

                    break;
                case WorkStage.DownLift:
                    break;
                case WorkStage.Right:
                    if (ReachedTurnEnd(true))
                    {
                        _stage = WorkStage.Forward;
                    }
                    else
                    {
                        TurnRightReversed(true);
                    }

                    break;
                case WorkStage.Left:
                    if (ReachedTurnEnd(true))
                    {
                        _stage = WorkStage.Forward;
                    }
                    else
                    {
                        TurnRightReversed(false);
                    }

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

        bool ReachedTurn(bool _forward)
        {
            var d = turnPos; // forward
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

        bool ReachedTurnEnd(bool _forward)
        {
            var d = turnEndPos; // forward
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
            var _acceleration = 0.5f;
            if (!forward)
            {
                _acceleration = -0.5f;
            }
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
            _vehicleController.SteeringInput = 0;

            _vehicleController.AccelerationInput = _acceleration;
        }

        void TurnRightReversed(bool Right)
        {
            var _acceleration = -0.25f;
            var _steering = 1.0f;
            if (!Right)
            {
                _steering = -1.0f;
            }
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
            _vehicleController.SteeringInput = 0;

            _vehicleController.AccelerationInput = _acceleration;
            _vehicleController.SteeringInput = _steering;
        }

        public void Animate()
        {
            moving = true;

            _stage = WorkStage.UpLift;
        }
    }
}