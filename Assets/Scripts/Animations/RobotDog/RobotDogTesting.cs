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
        Pickup = 4,
        Dropoff = 5,
    }

    public class RobotDogTesting : MonoBehaviour
    {
        public GameObject robotDog;
        public GameObject pipe;

        public RobotDogInputRecording recording;
        public RobotDogInputReplay replay;

        private RobotStage stage;

        private float angleThreshold = 2f;
        private float distanceThreshold = 0.5f;

        private void Start()
        {
            stage = RobotStage.Stop;
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

        public void Update()
        {
            var angle = Math.Abs(GetForwardAngleDiff());
            var pipet = pipe.transform;

            var rot1 = pipe.transform.rotation.eulerAngles.y;
            var rot2 = robotDog.transform.rotation.eulerAngles.y;

            var rotDiff = rot1 - rot2;

            print($"{rot1}\t{rot2}\t{rotDiff}");

            var yoffset = replay.rotationOffset;

            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Forward:
                    var distance = GetDistance(pipet);

                    if (distance < distanceThreshold)
                    {
                        replay.Forward(false, true);
                        // stage = RobotStage.Stop;
                        // force update position
                        ForceRobotPosition(pipet);
                        // make it to pickup
                        stage = RobotStage.Pickup;
                    }
                    else
                    {
                        replay.Forward(true);
                    }

                    break;

                case RobotStage.Left:
                    if (angle < angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.LeftTurn(false, true);
                        // stage = RobotStage.Stop;
                        ForceRobotTowards(pipet);
                        print("left stop");
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        replay.LeftTurn(true, false);
                    }

                    break;
                case RobotStage.Right:
                    if (angle < angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.RightTurn(false, true);
                        print("right stop");
                    }
                    else
                    {
                        replay.RightTurn(true, false);
                    }

                    break;

                case RobotStage.Pickup:
                    if (rotDiff < yoffset)
                    {
                        if (yoffset - rotDiff < angleThreshold)
                        {
                            // stop
                            stage = RobotStage.Stop;
                            replay.RightTurn(false, true);
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
                            stage = RobotStage.Stop;
                            replay.LeftTurn(false, true);
                        }
                        else
                        {
                            // left turn
                            replay.LeftTurn(true);
                        }
                    }

                    break;
            }
        }

        float GetForwardAngleDiff()
        {
            var pos1 = robotDog.transform.position;
            var pos2 = pipe.transform.position;

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
            var pos1 = robotDog.transform.position;
            var pos2 = t.position;

            var zoffset = replay.positionOffset;

            pos1.x = pos2.x;
            pos1.z = pos2.z - zoffset;

            robotDog.transform.position = pos1;
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Move"))
            {
                // move to target
                MoveToTarget();
            }

            if (GUI.Button(new Rect(10, 150, 100, 50), "Rotate"))
            {
                replay.LeftTurn(false);
            }
        }
    }
}