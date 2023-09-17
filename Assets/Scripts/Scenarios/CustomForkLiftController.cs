using System;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
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
        public Transform turn2;
        public Transform turnEnd2;
        public Transform turn3;
        public Transform turnEnd3;

        private WSMVehicleController _vehicleController;
        private ForkliftController _forkliftController;

        private Vector3 destinationPos;

        private Vector3 turnPos;
        private Vector3 turnPos2;
        private Vector3 turnPos3;

        private Vector3 turnEndPos;
        private Vector3 turnEndPos2;
        private Vector3 turnEndPos3;

        private Vector3 startPos;
        private Quaternion startRotation;

        private Vector3 goodStartPos;
        private Quaternion goodStartRotation;

        private WorkStage _stage;
        private float liftHeightThreshold = 0.65f;

        private bool moving = false;
        private bool returning = false;
        private float distanceThreshold = 2.0f;


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
            turnPos2 = turn2.transform.position;
            turnEndPos2 = turnEnd2.transform.position;
            turnPos3 = turn3.transform.position;
            turnEndPos3 = turnEnd3.transform.position;

            goodStartPos = good.transform.position;
            goodStartRotation = good.transform.rotation;

            _vehicleController = forklift.GetComponent<WSMVehicleController>();
            _forkliftController = forklift.GetComponent<ForkliftController>();
        }

        private void Update()
        {
            print(_stage);

            if (!moving) return;

            switch (_stage)
            {
                case WorkStage.Stop:
                    
                    if (!returning)
                    {
                        StallVehicle();
                    }
                    else if (returning)
                    {
                        StopResetVehicle();
                    }

                    break;
                case WorkStage.UpLift:
                    if (ReachedLiftHeight(true) && !returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else if (ReachedTurnEnd3(true) && returning)
                    {
                        _stage = WorkStage.Left;
                    }
                    else if (ReachedLiftHeight(true) && returning)
                    {
                       StartVehicle();
                       ReverseTurnRight(true);
                    }
                    else
                    {
                        LiftLoad(true);
                    }

                    break;
                case WorkStage.Forward:
                    if (ReachedDestination(true) && !returning)
                    {
                        // stop
                        _stage = WorkStage.Stop;
                    }
                    else if (ReachedTurn(true) && !returning)
                    {
                        _stage = WorkStage.Left;
                    }
                    else if (ReachedTurn2(true) && !returning)
                    {
                        _stage = WorkStage.Left;
                    }
                    else if (ReachedTurn(true) && returning)
                    {
                        _stage = WorkStage.Right;
                    }
                    else if (ReachedTurn2(true) && returning)
                    {
                        _stage = WorkStage.Right;
                    }
                    else
                    {
                        MoveForward(true);
                    }

                    break;
                case WorkStage.Back:
                    if (ReachedTurn(true) && !returning)
                    {
                        _stage = WorkStage.Left;
                    }
                    else if (ReachedTurn3(true) && returning)
                    {
                        StopVehicle();
                        _stage = WorkStage.UpLift;
                    }
                    else
                    {
                        MoveForward(false);
                    }

                    break;
                case WorkStage.DownLift:
                    
                    LiftLoad(false);

                    if (ReachedLiftHeight(false))
                    {
                        StartVehicle();
                        good.transform.parent = null;
                        returning = true;
                        _stage = WorkStage.Back;
                    }

                    break;
                case WorkStage.Right:
                    if (ReachedTurnEnd(true) && !returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else if (ReachedTurnEnd2(true) && !returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else if (ReachedStart(true) && returning)
                    {
                        _stage = WorkStage.Stop;
                    }
                    else if (ReachedTurnEnd(true) && returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else
                    {
                        TurnRight(true);
                    }

                    break;
                case WorkStage.Left:
                    if (ReachedTurnEnd(true) && !returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else if (ReachedTurnEnd2(true) && !returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else if (ReachedTurnEnd2(true) && returning)
                    {
                        _stage = WorkStage.Forward;
                    }
                    else
                    {
                        TurnRight(false);
                    }

                    break;
            }
        }
        
        void StallVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;

            print(_vehicleController.CurrentSpeed.ToString("f5"));
            if (_vehicleController.CurrentSpeed < 0.1)
            {
                _stage = WorkStage.DownLift;
            }
        }

        void StopVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;

            print(_vehicleController.CurrentSpeed.ToString("f5"));
        }

        void StopResetVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;

            print(_vehicleController.CurrentSpeed.ToString("f5"));
            if (_vehicleController.CurrentSpeed < 0.1)
            {
                ResetStatus();
            }
        }

        void StartVehicle()
        {
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;

            print(_vehicleController.CurrentSpeed.ToString("f5"));
       
        }

        private void ResetStatus()
        {
            _forkliftController.forks.ResetToMin();
            _forkliftController.ForksVertical = 0;
            
            forklift.transform.position = startPos;
            forklift.transform.rotation = startRotation;

            good.transform.position = goodStartPos;
            good.transform.rotation = goodStartRotation;
            good.transform.parent = forklift.transform;

            returning = false;

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

        bool ReachedStart(bool _forward)
        {
            var d = startPos; // forward
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

        bool ReachedTurn2(bool _forward)
        {
            var d = turnPos2; // forward
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

        bool ReachedTurn3(bool _forward)
        {
            var d = turnPos3; // forward
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

        bool ReachedTurnEnd2(bool _forward)
        {
            var d = turnEndPos2; // forward
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

        bool ReachedTurnEnd3(bool _forward)
        {
            var d = turnEndPos3; // forward
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
            var _acceleration = 0.25f;
            if (!forward)
            {
                _acceleration = -0.25f;
            }
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
            _vehicleController.SteeringInput = 0;

            _vehicleController.AccelerationInput = _acceleration;
        }

        void TurnRight(bool Right)
        {
            var _acceleration = 0.25f;
            var _steering = 0.75f;
            if (!Right)
            {
                _steering = -0.75f;
            }
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
            _vehicleController.SteeringInput = 0;

            _vehicleController.AccelerationInput = _acceleration;
            _vehicleController.SteeringInput = _steering;
        }

        void ReverseTurnRight(bool Right)
        {
            var _acceleration = -0.25f;
            var _steering = 0.75f;
            if (!Right)
            {
                _steering = -0.75f;
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