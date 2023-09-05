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

        public void Update()
        {
            var angle = Math.Abs(GetForwardAngleDiff());
            print(angle);
            
            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Left:
                    if (angle < angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.LeftTurn(false, true);
                        stage = RobotStage.Stop;
                        ForceRobotTowards(pipe.transform);
                        print("left stop");
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