using System;
using UnityEngine;

namespace VRC2.Animations
{
    internal enum RobotStage
    {
        Stop = 0,
        Forward = 1,
        Left = 2,
        Right = 3,
        PickupPrepare = 4,
        Pickup = 5,
        Dropoff = 6,
    }

    public class RobotDogTesting : MonoBehaviour
    {
        public GameObject robotDog;
        public GameObject pipe;

        public GameObject attachePoint;

        public RobotDogInputRecording recording;
        public RobotDogInputReplay replay;

        private RobotStage stage;

        private float angleThreshold = 2f;
        private float distanceThreshold = 0.5f;

        private bool pickingup = false;
        private bool droppingoff = false;

        private GameObject targetGameObject;

        #region Targets

        public Transform bendcutMachine;
        public Transform bendcutOutput;
        public Transform deliveryPoint;

        private Transform targetTransform;

        #endregion

        private void Start()
        {
            stage = RobotStage.Stop;
            targetTransform = pipe.transform;
            
            recording.OnCloseGrip += OnCloseGrip;
        }

        private void OnCloseGrip()
        {
            if (pipe.transform.parent == null)
            {
                // fix grapping
                print("manually fix grapping");
                ManuallyFixGrabbing();
            }
        }

        void MoveToTarget()
        {
            // calculate the angle
            var angle = GetForwardAngleDiff();
            if (angle < 0)
            {
                // left turn
                stage = RobotStage.Left;
                replay.LeftTurn(true);
            }
            else
            {
                // right turn
                stage = RobotStage.Right;
                replay.RightTurn(true);
            }
        }

        float GetDistance(Transform t)
        {
            var pos1 = robotDog.transform.position;
            pos1.y = 0;
            var pos2 = t.position;
            pos2.y = 0;

            return Vector3.Distance(pos1, pos2);
        }

        // sometimes, it can not precisely grab the pipe
        void ManuallyFixGrabbing()
        {
            // disable gravity
            var rb = targetGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;   
            }
            
            // update position
            // var pos = targetGameObject.transform.position;
            // pos.y = attachePoint.transform.position.y;

            targetGameObject.transform.position = attachePoint.transform.position;
        }

        // Tip: better to use FixedUpdate than Update for animation replaying
        public void FixedUpdate()
        {
            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Forward:
                    var distance = GetDistance(targetTransform);
                    
                    print(distance);

                    if (distance < distanceThreshold)
                    {
                        replay.Forward(false, true);
                        // stage = RobotStage.Stop;
                        // force update position
                        // ForceRobotPosition(target);

                        if (targetTransform == pipe.transform)
                        {
                            targetGameObject = pipe;
                            // pickup
                            // make it to pickup
                            stage = RobotStage.PickupPrepare;
                        }
                        else if (targetTransform == bendcutMachine)
                        {
                            // make it dropoff
                            stage = RobotStage.Dropoff;
                            droppingoff = false;
                        }
                        else if (targetTransform == bendcutOutput)
                        {
                            print("forward to bend cut output");
                            targetGameObject = bendcutOutput.gameObject;
                            stage = RobotStage.PickupPrepare;
                        }
                        else if (targetTransform == deliveryPoint)
                        {
                            stage = RobotStage.Dropoff;
                            droppingoff = false;
                        }
                    }
                    else
                    {
                        replay.Forward(true);
                    }

                    break;

                case RobotStage.Left:
                    var angle = Math.Abs(GetForwardAngleDiff());
                    print($"left: {angle}");
                    if (angle < 2 * angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.LeftTurn(false, true);
                        ForceRobotTowards(targetTransform);
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        replay.LeftTurn(true, false);
                    }

                    break;
                case RobotStage.Right:
                    angle = Math.Abs(GetForwardAngleDiff());
                    print($"right: {angle}");
                    if (angle < 2 * angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.RightTurn(false, true);
                        ForceRobotTowards(targetTransform);
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        replay.RightTurn(true, false);
                    }

                    break;

                case RobotStage.PickupPrepare:
                    var f1 = targetTransform.forward;
                    var f2 = robotDog.transform.forward;

                    f1.y = 0;
                    f2.y = 0;

                    var rotDiff = Vector3.Angle(f1, f2);
                    var yoffset = replay.rotationOffset;
                    if (rotDiff < yoffset)
                    {
                        if (yoffset - rotDiff < angleThreshold)
                        {
                            // pickup
                            stage = RobotStage.Pickup;
                            pickingup = false;
                            replay.RightTurn(false, true);
                            ForceRobotPosition(targetTransform);
                        }
                        else
                        {
                            // right turn
                            replay.RightTurn(true);
                        }
                    }
                    else
                    {
                        if (rotDiff - yoffset < angleThreshold)
                        {
                            // stop
                            stage = RobotStage.Pickup;
                            pickingup = false;
                            replay.LeftTurn(false, true);
                            ForceRobotPosition(targetTransform);
                        }
                        else
                        {
                            // left turn
                            replay.LeftTurn(true);
                        }
                    }

                    break;

                case RobotStage.Pickup:
                    if (!pickingup)
                    {
                        if (recording.IsIdle())
                        {
                            replay.Pickup();
                            pickingup = true;
                        }
                        else
                        {
                            replay.Stop(true);
                        }
                    }
                    else
                    {
                        if (replay.PickupDone())
                        {
                            // move to target
                            print("pickup is done");
                            if (targetTransform == pipe.transform)
                            {
                                // change target to bendcut machine
                                targetTransform = bendcutMachine;
                            }
                            else if (targetTransform == bendcutOutput)
                            {
                                targetTransform = deliveryPoint;
                            }

                            MoveToTarget();
                        }
                    }

                    break;

                case RobotStage.Dropoff:
                    if (!droppingoff)
                    {
                        if (recording.IsIdle())
                        {
                            replay.Dropoff();
                            droppingoff = true;
                        }
                        else
                        {
                            replay.Stop(true);
                        }
                    }
                    else
                    {
                        if (replay.DropoffDone())
                        {
                            // reset arm
                            recording.ResetArm();

                            // TODO: make a timer to let it wait

                            if (targetTransform == bendcutMachine)
                            {
                                // ready to pickup again
                                targetTransform = bendcutOutput;

                                MoveToTarget();
                            }
                            else if (targetTransform == deliveryPoint)
                            {
                                stage = RobotStage.Stop;
                            }
                        }
                    }

                    break;
            }
        }

        float GetForwardAngleDiff()
        {
            var pos1 = robotDog.transform.position;
            var pos2 = targetTransform.position;

            pos1.y = 0;
            pos2.y = 0;

            var robotForward = robotDog.transform.forward;

            var angle = Vector3.SignedAngle(robotForward, pos2 - pos1, Vector3.up);
            return angle;
        }

        void ForceRobotTowards(Transform t)
        {
            var pos1 = robotDog.transform.position;
            var pos2 = t.position;

            var vec = pos2 - pos1;
            vec.y = 0;

            var upward = robotDog.transform.up;
            var rot = Quaternion.LookRotation(vec.normalized, upward);
            robotDog.transform.rotation = rot;
        }

        void ForceRobotPosition(Transform t)
        {
            var zoffset = replay.positionOffset;
            var pos = t.position;
            // use the original y
            pos.y = robotDog.transform.position.y;
            robotDog.transform.position = pos;
            robotDog.transform.Translate(0, 0, -zoffset, Space.Self);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Move"))
            {
                // move to target
                MoveToTarget();
            }

            if (GUI.Button(new Rect(150, 10, 100, 50), "Rotate"))
            {
                replay.LeftTurn(false);
            }

            if (GUI.Button(new Rect(10, 100, 100, 50), "ResetArm"))
            {
                recording.ResetArm();
            }
            
            if (GUI.Button(new Rect(150, 100, 100, 50), "Pickup"))
            {
                replay.RewindPickup();
                replay.Pickup();
            }
        }
    }
}