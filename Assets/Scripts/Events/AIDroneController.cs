using System;
using System.Collections;
using System.Collections.Generic;
using PA_DronePack;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Events;

namespace VRC2
{
    enum DroneStatus
    {
        Stop = 0, // motor stopped
        Run = 1, // motor ran
        Lift = 2, // go up 
        Move = 3, // move forward/backward
        Turn = 4, // turn around
        Down = 5 // go down
    }

    enum DroneRoutine
    {
        PickUp = 0,
        Dropoff = 1,
        Return = 2,
    }

    [RequireComponent(typeof(PA_DroneController))]
    public class AIDroneController : MonoBehaviour
    {
        [Header("Key Points")] public GameObject droneBase; // where is the drone locate
        public GameObject pipeWarehouse; // where to get the pipe
        public GameObject pipeStorage; // where to deliver the pipe

        [Header("Fly Settings")] public float flyHeight = 10.0f;

        public float moveForce = 0.5f;
        private float moveForceSmall = 0.1f;
        public float turnForce = 0.05f;
        public float liftForce = 0.2f;

        private float heightThreshold = 0.5f;
        private float angleThrehold = 3f;
        private float distanceThrehold = 10.0f;
        private float distanceThreholdSmall = 3f;

        // actions

        public System.Action ReadyToPickUp;
        public System.Action ReadyToDropOff;
        public System.Action ReadyToReturnToBase;

        private PA_DroneController _controller;

        private DroneStatus _status;

        private DroneRoutine _routine;

        private Vector3 _objective;

        // backup transform
        private Vector3 _backupPosition;
        private Quaternion _backupRotation;


        // Start is called before the first frame update
        void Start()
        {
            _controller = gameObject.GetComponent<PA_DroneController>();

            // backup
            _backupPosition = transform.position;
            _backupRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            switch (_status)
            {
                case DroneStatus.Lift:
                    var y = transform.position.y;
                    if (y >= flyHeight)
                    {
                        Lift(0);
                        // turn and move forward
                        _status = DroneStatus.Turn;
                        Turn(turnForce);
                    }

                    break;
                case DroneStatus.Turn:
                    // calculate direction
                    var direction = GetDirection();

                    var angle = Vector3.Angle(direction, transform.forward);

                    if (angle < angleThrehold)
                    {
                        // move forward
                        Turn(0);
                        _status = DroneStatus.Move;
                        ForwardBack(moveForce);
                    }

                    break;

                case DroneStatus.Move:
                    // calculate distance
                    var distance = GetDistance();

                    print(distance);

                    // use two-thresholds to fix passing by problem
                    if (distance < distanceThrehold)
                    {
                        if (distance > distanceThreholdSmall)
                        {
                            // move slower
                            ForwardBack(moveForceSmall);
                        }
                        else
                        {
                            // go down
                            ForwardBack(0);
                            _status = DroneStatus.Down;
                            Lift(-liftForce);
                        }
                    }

                    break;

                case DroneStatus.Down:
                    // calculate height from the
                    var height = GetHeight();
                    if (height < heightThreshold)
                    {
                        // drop off
                        if (_routine == DroneRoutine.PickUp)
                        {
                            print("pick up");
                            if (ReadyToPickUp != null) ReadyToPickUp();
                            // DropOff();
                        }
                        else if (_routine == DroneRoutine.Dropoff)
                        {
                            print("drop off");
                            if (ReadyToDropOff != null) ReadyToDropOff();
                            // ReturnToBase();
                        }
                        else if (_routine == DroneRoutine.Return)
                        {
                            if (ReadyToReturnToBase != null) ReadyToReturnToBase();
                            print("return to base");
                            // RunMotor(false);
                        }

                    }

                    break;
                case DroneStatus.Stop:
                    RestoreTransform();
                    break;

                default:
                    break;
            }
        }

        float GetHeight()
        {
            return Math.Abs(transform.position.y - _objective.y);
        }

        float GetDistance()
        {
            var pos = transform.position;
            pos.y = _objective.y;

            return Vector3.Distance(pos, _objective);
        }

        Vector3 GetDirection()
        {
            var v1 = new Vector3(_objective.x, 0, _objective.z);
            var v2 = transform.position;
            v2.y = 0;

            return v1 - v2;
        }

        void ForwardBack(float value)
        {
            _controller.DriveInput(value);
        }

        void RightLeft(float value)
        {
            _controller.StrafeInput(value);
        }

        void Lift(float value)
        {
            _controller.LiftInput(value);
        }

        void Turn(float value)
        {
            _controller.TurnInput(value);
        }

        void RunMotor(bool flag)
        {
            _controller.motorOn = flag;
            if (flag)
            {
                _status = DroneStatus.Run;
            }
            else
            {
                _status = DroneStatus.Stop;
            }
        }


        void MoveTo(Vector3 pos)
        {
            _objective = pos;
            // start motor
            RunMotor(true);
            // lift till the expected height
            _status = DroneStatus.Lift;
            Lift(liftForce);
        }


        public void PickUp()
        {
            print("pickup");
            _routine = DroneRoutine.PickUp;
            MoveTo(pipeWarehouse.transform.position);
        }

        public void DropOff()
        {
            _routine = DroneRoutine.Dropoff;
            MoveTo(pipeStorage.transform.position);
        }

        public void ReturnToBase()
        {
            _routine = DroneRoutine.Return;
            MoveTo(droneBase.transform.position);
        }

        public void Stop()
        {
            _controller.motorOn = false;
            _status = DroneStatus.Stop;
        }

        private void RestoreTransform()
        {
            while (Vector3.Distance(transform.position, _backupPosition) > 0.05f)
            {
                transform.position = _backupPosition;
                transform.rotation = _backupRotation;
            }
        }
    }
}