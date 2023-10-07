using System;
using UnityEngine;

using System;
using Fusion;
using Oculus.Interaction.DistanceReticles;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using VRC2.Events;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using AgentHelper = VRC2.Agent.AgentHelper;
using PipeParameters = VRC2.Pipe.PipeConstants.PipeParameters;
using PipeColor = VRC2.Pipe.PipeConstants.PipeColor;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;

namespace VRC2.Animations
{
    internal enum RobotStage
    {
        Default = -1,
        Stop = 0,
        Forward = 1,
        Left = 2,
        Right = 3,
        PickupRotate = 41,
        PickupStrafe = 42,
        Pickup = 5,
        Dropoff = 6,
    }

    internal enum StrafeDirection
    {
        Unknown = -1,
        Left = 1,
        Right = 2,
    }

    public class RobotDogController : MonoBehaviour
    {
        public GameObject robotDog;
        private Animator dogAnimator;

        public GameObject attachePoint;

        public RobotDogInputRecording recording;
        public RobotDogInputReplay replay;

        private RobotStage stage;

        private float angleThreshold = 1f;
        private float distanceThreshold = 0.1f;

        private bool pickingup = false;
        private bool droppingoff = false;

        private GameObject targetGameObject;

        #region Targets

        [Space(30)] [Header("Target")] public Transform bendcutInput;
        public Transform bendcutOutput;
        public Transform deliveryPoint;
        public Transform standbyPoint;

        private Transform targetTransform;

        [Space(30)] [Header("Arm")] public GameObject robotArmRoot;

        private RoboticArm roboticArm;
        private Animator armAnimator;

        #endregion

        #region For network

        public GameObject currentPipe { get; set; }

        private PipeParameters parameters;

        private NetworkObject spawnedPipe;

        public System.Action<PipeBendAngles> ReadyToOperate;

        #endregion

        #region Robot body control

        [Space(30)] [Header("Body")]
        //Movement
        public float moveSpeed = 1f;

        public float strafeSpeed = 0.1f;

        public float rotateSpeed = 1f;

        //
        private Transform body
        {
            get => robotDog.transform;
        }

        //Scripts
        public Actions actions;

        private bool walk = false;
        private bool turn = false;

        private float rotationOffset = 90; // pipe.y - dog.y

        // private bool pickupDone = false;
        // private bool dropoffDone = false;

        private StrafeDirection strafeDirection = StrafeDirection.Unknown;

        #endregion

        #region Compensate rotation when preparing pickup

        private Vector3 GetCompensateForward(Transform t)
        {
            var pm = t.gameObject.GetComponent<PipeManipulation>();

            // rotate forward vector
            // refer: https://discussions.unity.com/t/rotate-a-vector3-direction/14722
            // vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;
            // vector = Quaternion.Euler(0, -45, 0) * vector;

            var forward = t.forward;

            if (pm.angle == PipeBendAngles.Angle_45)
            {
                forward = Quaternion.Euler(0, 90, 0) * forward;
            }

            return forward;
        }

        #endregion

        private void Start()
        {
            stage = RobotStage.Default;

            dogAnimator = robotDog.GetComponent<Animator>();

            // set arm reference
            replay.arm = recording.arm;
            replay.recording = recording;

            recording.OnCloseGripOnce += OnCloseGripOnce;
            recording.OnNeedReleasingOnce += OnNeedReleasingOnce;

            armAnimator = robotArmRoot.GetComponent<Animator>();
            roboticArm = robotArmRoot.GetComponent<RoboticArm>();

            roboticArm.ReadyToPickup += ReadyToPickup;
            roboticArm.ReadyToDropoff += ReadyToDropoff;

        }

        private void ReadyToDropoff()
        {
            print("ReadyToDropoff");
            var pipe = GlobalConstants.lastSpawnedPipe;

            // Add rigid body, etc.
            PipeHelper.AfterMove(ref pipe);
            // update box colliders
            PipeHelper.UpdateBoxColliders(pipe, true);
        }

        private void ReadyToPickup()
        {
            print("ReadyToPickup");
            var pipe = GlobalConstants.lastSpawnedPipe;

            pipe.transform.parent = attachePoint.transform;
            pipe.transform.localPosition = Vector3.zero;
            pipe.transform.localRotation = Quaternion.identity;
        }

        #region Gripper one-time callback

        private void OnNeedReleasingOnce()
        {
            targetGameObject.transform.parent = null;
            PipeHelper.AfterMove(ref targetGameObject);
        }

        // this is only triggered once
        private void OnCloseGripOnce()
        {
            // sometimes, it can not precisely grab the pipe
            print("manually fix grapping");
            // var pos = attachePoint.transform.position;
            // pos.y = targetTransform.transform.position.y;
            // targetGameObject.transform.position = pos;
            var pipe = GlobalConstants.lastSpawnedPipe;

            if (pipe == null) return;


            // remove rigid body
            PipeHelper.BeforeMove(ref pipe);
            // update box colliders
            PipeHelper.UpdateBoxColliders(pipe, false);

            pipe.transform.parent = attachePoint.transform;
            pipe.transform.localPosition = Vector3.zero;
            pipe.transform.localRotation = Quaternion.identity;
        }

        #endregion

        #region Animation control


        void MoveToTarget()
        {
            // make it only turn left
            stage = RobotStage.Left;
        }

        float GetDistance(Transform t)
        {
            var pos1 = robotDog.transform.position;
            pos1.y = 0;
            var pos2 = t.position;
            pos2.y = 0;

            return Vector3.Distance(pos1, pos2);
        }

        #region Robot dog body control

        void RobotStrafe()
        {
            if (!walk)
            {
                walk = true;
                actions.Walk();
            }

            if (strafeDirection == StrafeDirection.Unknown)
            {
                var forward = body.transform.forward;

                var vec = targetTransform.position - body.transform.position;

                vec.y = 0;

                var angel = Vector3.Angle(forward, vec);

                if (angel < 45)
                {
                    strafeDirection = StrafeDirection.Right;
                }
                else
                {
                    strafeDirection = StrafeDirection.Left;
                }
            }

            var x = -1;
            if (strafeDirection == StrafeDirection.Right)
            {
                // right
                actions.StrafeRight();
                x = 1;
            }
            else if (strafeDirection == StrafeDirection.Left)
            {
                // left
                actions.StrafeLeft();
            }

            body.Translate(new Vector3(x, 0, 0) * strafeSpeed * Time.deltaTime);
        }

        bool StrafeRobot()
        {
            var distance = GetDistance(targetTransform);
            // 0.15 is to make robot is just above the pipe
            if (distance < 0.15f)
            {
                return true;
            }

            RobotStrafe();
            return false;
        }

        bool TurnLeft()
        {
            if (Math.Abs(GetForwardAngleDiff()) < angleThreshold)
            {
                print("TurnLeft is done");
                actions.Idle1();
                turn = false;

                // return true is turn is done
                return true;
            }

            if (!turn)
            {
                turn = true;
                actions.TurnLeft();
            }

            // rotate rigid body
            var pos1 = robotDog.transform.position;
            var pos2 = targetTransform.position;
            pos1.y = 0;
            pos2.y = 0;

            var targetRot = Quaternion.LookRotation(pos2 - pos1, body.transform.up);

            var rb = body.transform.rotation;

            var newRb = Quaternion.RotateTowards(rb, targetRot, Time.deltaTime * rotateSpeed);

            body.transform.rotation = newRb;

            return false;

        }

        bool TurnLeftUntil(Vector3 angle, float yoffset)
        {
            if (!turn)
            {
                turn = true;
                actions.TurnLeft();
            }

            // add y offset
            angle.y += yoffset;
            var rot = Quaternion.Euler(angle);
            var newRot = Quaternion.RotateTowards(body.transform.rotation, rot, Time.deltaTime * rotateSpeed);
            body.transform.rotation = newRot;

            var diff = Vector3.SignedAngle(body.transform.forward, targetTransform.forward, Vector3.up);

            if (Math.Abs(Math.Abs(diff) - rotationOffset) < angleThreshold)
            {
                return true;
            }

            return false;
        }

        void TurnRight()
        {
            if (!turn)
            {
                turn = true;
                actions.TurnRight();
            }

            body.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
        }

        bool MoveForward()
        {
            var distance = GetDistance(targetTransform);
            print($"distance {distance}");
            if (distance < distanceThreshold)
            {
                return true;
            }

            if (!walk)
            {
                walk = true;
                actions.Walk();
            }

            // rotate rigid body
            var pos1 = robotDog.transform.position;
            var pos2 = targetTransform.position;
            pos2.y = pos1.y;

            var newPos = Vector3.MoveTowards(pos1, pos2, Time.deltaTime * moveSpeed);

            body.transform.position = newPos;

            return false;
        }

        void MoveBackward()
        {
            if (!walk)
            {
                walk = true;
                actions.Walk();
            }

            body.Translate(new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime);
        }

        void Idle()
        {
            walk = false;
            turn = false;
            actions.Idle1();
        }

        #endregion


        // Tip: better to use FixedUpdate than Update for animation replaying
        private void Update()
        {
            if (stage == RobotStage.Default) return;

            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Forward:

                    print("forward");
                    // qForceRobotTowards(targetTransform);
                    if (MoveForward())
                    {
                        print("MoveForward is done");
                        Idle();

                        if (currentPipe != null && targetTransform == currentPipe.transform)
                        {
                            // pickup
                            // make it ready for pickup
                            stage = RobotStage.PickupRotate;
                        }
                        else if (targetTransform == bendcutInput)
                        {
                            // // make it dropoff
                            // stage = RobotStage.Dropoff;
                            // droppingoff = false;
                            stage = RobotStage.Dropoff;
                        }
                        else if (targetTransform == bendcutOutput)
                        {
                            print("forward to bend cut output");
                            stage = RobotStage.PickupRotate;
                        }
                        else if (targetTransform == deliveryPoint)
                        {
                            stage = RobotStage.Dropoff;
                            droppingoff = false;
                        }
                        else if (targetTransform == standbyPoint)
                        {
                            stage = RobotStage.Stop;
                            // reset arm
                            recording.ResetArm();
                        }
                    }

                    break;

                case RobotStage.Left:
                    print("left");
                    if (TurnLeft())
                    {
                        print("turn left is done");
                        stage = RobotStage.Forward;
                    }

                    break;
                case RobotStage.Right:
                    var angle = Math.Abs(GetForwardAngleDiff());
                    print($"right: {angle}");
                    if (angle < angleThreshold)
                    {
                        Idle();
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        TurnRight();
                    }

                    break;

                case RobotStage.PickupStrafe:

                    if (StrafeRobot())
                    {
                        print("strafe is done");
                        // force update position and rotation for precise grap
                        FinetuneRobotPosition(targetTransform);
                        Idle();
                        pickingup = false;
                        stage = RobotStage.Pickup;
                    }

                    break;

                case RobotStage.PickupRotate:

                    // make turn left again
                    // var f1 = targetTransform.forward;
                    print("prepare");
                    var f1 = GetCompensateForward(targetGameObject.transform);
                    // var f2 = robotDog.transform.forward;

                    // f1.y = 0;
                    // f2.y = 0;

                    if (TurnLeftUntil(f1, rotationOffset))
                    {
                        print("TurnLeftUntil is done");
                        strafeDirection = StrafeDirection.Unknown;
                        // strafe
                        stage = RobotStage.PickupStrafe;
                    }



                    // var rotDiff = Vector3.Angle(f1, f2);
                    // var yoffset = replay.rotationOffset;
                    // if (rotDiff < yoffset)
                    // {
                    //     if (yoffset - rotDiff < angleThreshold)
                    //     {
                    //         // pickup
                    //         stage = RobotStage.Pickup;
                    //         pickingup = false;
                    //         // replay.RightTurn(false, true);
                    //         ForceRobotPosition(targetTransform);
                    //     }
                    //     else
                    //     {
                    //         // // right turn
                    //         // replay.RightTurn(true);
                    //         TurnLeft();
                    //     }
                    // }
                    // else
                    // {
                    //     if (rotDiff - yoffset < angleThreshold)
                    //     {
                    //         // stop
                    //         stage = RobotStage.Pickup;
                    //         pickingup = false;
                    //         replay.LeftTurn(false, true);
                    //         ForceRobotPosition(targetTransform);
                    //     }
                    //     else
                    //     {
                    //         // left turn
                    //         replay.LeftTurn(true);
                    //     }
                    // }

                    break;

                case RobotStage.Pickup:
                    if (!pickingup)
                    {
                        print("start pickup");
                        pickingup = true;
                        // disable dog animator, otherwise arm animator won't work.
                        dogAnimator.enabled = false;
                        armAnimator.enabled = true;

                        StartPickupAnimation();
                    }
                    else
                    {
                        if (IsPickupDone())
                        // if (pickupDone)
                        {
                            // move to target
                            print("pickup is done");

                            // enable dog animator, disable arm animator
                            armAnimator.enabled = false;
                            dogAnimator.enabled = true;

                            turn = false;
                            walk = false;

                            droppingoff = false;

                            if (currentPipe != null && targetTransform == currentPipe.transform)
                            {
                                // change target to bendcut machine
                                targetTransform = bendcutInput;
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
                        print("drop off");
                        droppingoff = true;

                        dogAnimator.enabled = false;
                        armAnimator.enabled = true;

                        StartDropoffAnimation();
                        // if (recording.IsIdle())
                        // {
                        //     replay.Dropoff();
                        //     droppingoff = true;
                        // }
                        // else
                        // {
                        //     replay.Stop(true);
                        // }
                    }
                    else
                    {
                        if (IsDropoffDone())
                        // if (dropoffDone)
                        {
                            print("dropoff done");
                            droppingoff = false;
                            // reset arm
                            UpdateAnimator(true, false, false);

                            turn = false;
                            walk = false;

                            if (targetTransform == bendcutInput)
                            {
                                print("bend cut output");

                                stage = RobotStage.Stop;

                                // ready to pickup again
                                targetTransform = bendcutOutput;

                                // make it waiting
                                if (ReadyToOperate != null)
                                {
                                    ReadyToOperate(parameters.angle);
                                }
                            }
                            else if (targetTransform == deliveryPoint)
                            {
                                targetTransform = standbyPoint;
                                MoveToTarget();
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

        void FinetuneRobotPosition(Transform t)
        {
            var pos = body.transform.position;
            pos.z = t.position.z;
            body.transform.position = pos;
        }


        #endregion

        #region Network synchronization



        void UpdateLocalSpawnedPipe(GameObject go)
        {
            var pm = go.GetComponent<PipeManipulation>();

            // set material
            pm.SetMaterial(parameters);
            // set length
            pm.SetLength(parameters.a, parameters.b);
        }

        // update spawned pipe since it might be different from the prefab
        void UpdateRemoteSpawnedPipe(NetworkId nid, PipeType type, PipeColor color, float a,
            float b)
        {
            var runner = GlobalConstants.networkRunner;
            var go = runner.FindObject(nid).gameObject;

            // update material
            var pm = go.GetComponent<PipeManipulation>();

            // set material
            pm.SetMaterial(type, color);
            // set length
            pm.SetLength(a, b);
        }

        void SpawnPipeUsingSelected()
        {
            // unparent
            currentPipe.transform.parent = null;

            var t = currentPipe.transform;
            var pos = t.position;
            var rot = t.rotation;
            // var scale = t.localScale;

            // destroy
            GameObject.DestroyImmediate(currentPipe);
            GlobalConstants.lastSpawnedPipe = null;


            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            var prefab = PipeHelper.GetPipePrefabRef(parameters);

            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeType type, PipeColor color, float a,
            float b,
            RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"RobotBendCut message ({type}, {color}, {a}, {b})\n";
                Debug.LogWarning(message);
            }
            else
            {
                message = $"RobotBendCut received message ({type}, {color}, {a}, {b})\n";
                Debug.LogWarning(message);

                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, type, color, a, b);
            }
        }


        public void InitParameters(PipeBendAngles angle, float a, float b)
        {
            parameters.angle = angle;
            parameters.a = a;
            parameters.b = b;
            // color and type and from global constants
            // get color and type
            var pm = currentPipe.GetComponent<PipeManipulation>();
            parameters.color = pm.pipeColor;
            parameters.type = pm.pipeType;
            parameters.diameter = pm.diameter;
        }

        public void PickUp()
        {
            Debug.Log("Robot PickUp");

            // remove rigidbody
            targetGameObject = currentPipe;
            PipeHelper.BeforeMove(ref targetGameObject);
            PipeHelper.UpdateBoxColliders(targetGameObject, false);

            // get the pipe from P1, and move to pickup the pipe
            targetTransform = targetGameObject.transform;
            MoveToTarget();
        }

        public void Execute()
        {
            PickUp();
        }

        public NetworkObject SpawnPipe(PipeBendAngles angle)
        {
            // update angle
            parameters.angle = angle;

            // start bend/cut
            SpawnPipeUsingSelected();
            UpdateLocalSpawnedPipe(spawnedPipe.gameObject);
            // update remote object
            RPC_SendMessage(spawnedPipe.Id, parameters.type, parameters.color, parameters.a, parameters.b);
            return spawnedPipe;
        }

        // NOTE: gameobject may be different from the transform, the transform is to mark the destination
        public void PickupResult(GameObject go, Transform t)
        {
            // move to pick result
            print("pickup result");
            PipeHelper.BeforeMove(ref go);
            targetGameObject = go;
            targetTransform = t;

            // enable dog animator, disable arm animator
            armAnimator.enabled = false;
            dogAnimator.enabled = true;

            MoveToTarget();
        }

        #endregion

        #region GUI Debug



        private void OnGUI()
        {
            // if (GUI.Button(new Rect(10, 10, 100, 50), "Move"))
            // {
            //     // move to target
            //     MoveToTarget();
            // }
            //
            // if (GUI.Button(new Rect(150, 10, 100, 50), "Rotate"))
            // {
            //     replay.LeftTurn(false);
            // }
            //
            // if (GUI.Button(new Rect(10, 100, 100, 50), "Pickup Prep"))
            // {
            //     stage = RobotStage.PickupRotate;
            // }
            //
            // if (GUI.Button(new Rect(150, 100, 100, 50), "Pickup"))
            // {
            //     stage = RobotStage.Default;
            //     replay.RewindPickup();
            //     replay.Pickup();
            // }
            //
            // if (GUI.Button(new Rect(10, 150, 100, 50), "Dropoff"))
            // {
            //     stage = RobotStage.Dropoff;
            // }

            if (GUI.Button(new Rect(10, 200, 100, 50), "Pickup"))
            {
                // StartPickupAnimation();
                PickUp();
            }

            if (GUI.Button(new Rect(150, 200, 100, 50), "Idle"))
            {
                StopAnimation();
            }

            if (GUI.Button(new Rect(300, 200, 100, 50), "Dropoff"))
            {
                StartDropoffAnimation();
            }
        }

        #endregion

        #region RobotArm Animation Control

        void UpdateAnimator(bool idle, bool pickup, bool dropoff)
        {
            armAnimator.SetBool("ArmIdle", idle);
            armAnimator.SetBool("Pickup", pickup);
            armAnimator.SetBool("Dropoff", dropoff);
        }


        void StartPickupAnimation()
        {
            UpdateAnimator(false, true, false);
        }

        void StopAnimation()
        {
            UpdateAnimator(true, false, false);
        }

        void StartDropoffAnimation()
        {
            UpdateAnimator(false, false, true);
        }
        // bool IsAnimationStatePlaying(Animator anim, int animLayer, string stateName)
        // {
        //     if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
        //         anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        //         return true;
        //     else
        //         return false;
        // }


        bool IsPickupDone()
        {
            return armAnimator.GetCurrentAnimatorStateInfo(0).IsName("RobotDogArmPickup") &&
                   armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
        bool IsDropoffDone()
        {
            return armAnimator.GetCurrentAnimatorStateInfo(0).IsName("RobotDogArmDropoff") &&
                   armAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }


        #endregion
    }
}