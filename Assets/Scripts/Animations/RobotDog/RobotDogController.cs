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
        PickupRotate = 3,
        PickupStrafe = 4,
        Pickup = 5,
        Dropoff = 6,
    }

    internal enum StrafeDirection
    {
        Unknown = -1,
        Left = 1,
        Right = 2,
    }

    public class RobotDogController : NetworkBehaviour
    {
        public GameObject robotDog;
        private Animator dogAnimator;

        public GameObject attachePoint;

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

        [Space(30)] [Header("Speed")] public float moveSpeed = 1f;

        public float strafeSpeed = 0.1f;

        public float rotateSpeed = 1f;

        //
        private Transform body
        {
            get => robotDog.transform;
        }

        //Scripts
        private Actions actions;

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

            armAnimator = robotArmRoot.GetComponent<Animator>();
            roboticArm = robotArmRoot.GetComponent<RoboticArm>();

            actions = robotDog.GetComponent<Actions>();

            roboticArm.ReadyToPickup += ReadyToPickup;
            roboticArm.ReadyToDropoff += ReadyToDropoff;

        }

        private void ReadyToDropoff()
        {
            print("ReadyToDropoff");
            var pipe = GlobalConstants.lastSpawnedPipe;

            // this will happen in Client side (p2)
            if (pipe == null) return;


            pipe.transform.parent = null;

            // Add rigid body, etc.
            PipeHelper.AfterMove(ref pipe);
            // update box colliders
            PipeHelper.UpdateBoxColliders(pipe, true);
        }

        private void ReadyToPickup()
        {
            print("ReadyToPickup");
            var pipe = GlobalConstants.lastSpawnedPipe;

            // this will happen in Client side (p2)
            if (pipe == null) return;

            PipeHelper.BeforeMove(ref pipe);
            PipeHelper.UpdateBoxColliders(pipe, false);

            pipe.transform.parent = attachePoint.transform;
            
            // don't change the local position and local rotation
            // pipe.transform.localPosition = Vector3.zero;
            // pipe.transform.localRotation = Quaternion.identity;
        }

        #region Animation control


        void MoveToTarget()
        {
            // make it only turn left
            stage = RobotStage.Left;
        }

        float GetDistance(Transform t)
        {
            var pos1 = body.position;
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
            var diff = Math.Abs(GetForwardAngleDiff());
            print($"turnleft diff: {diff}");
            if (diff < angleThreshold)
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

            var pos1 = body.position;
            var pos2 = targetTransform.position;
            pos1.y = 0;
            pos2.y = 0;

            // update directly to speed up rotating
            var targetRot = Quaternion.LookRotation(pos2 - pos1, body.transform.up);

            var rb = body.transform.rotation;

            if (diff > 15 * angleThreshold)
            {
                targetRot = Quaternion.RotateTowards(rb, targetRot, Time.deltaTime * rotateSpeed);
            }

            body.transform.rotation = targetRot;

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

            var pos1 = body.position;
            var pos2 = targetTransform.position;
            pos2.y = pos1.y;

            var newPos = Vector3.MoveTowards(pos1, pos2, Time.deltaTime * moveSpeed);

            body.transform.position = newPos;

            return false;
        }

        void Idle()
        {
            walk = false;
            turn = false;
            actions.Idle1();
        }

        #endregion

        private void Update()
        {
            if (stage == RobotStage.Default) return;

            switch (stage)
            {
                case RobotStage.Stop:
                    if (armAnimator.enabled)
                    {
                        armAnimator.enabled = false;
                    }

                    if (!dogAnimator.enabled)
                    {
                        dogAnimator.enabled = true;
                    }

                    Idle();
                    break;

                case RobotStage.Forward:

                    print("forward");
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
                            // make it dropoff
                            stage = RobotStage.Dropoff;
                        }
                        else if (targetTransform == bendcutOutput)
                        {
                            print("forward to bend cut output");
                            stage = RobotStage.PickupRotate;
                        }
                        else if (targetTransform == deliveryPoint)
                        {
                            print("delivery point");
                            stage = RobotStage.Dropoff;
                            droppingoff = false;
                        }
                        else if (targetTransform == standbyPoint)
                        {
                            print("standby point");
                            stage = RobotStage.Stop;
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
                    print("prepare");
                    var f1 = GetCompensateForward(targetGameObject.transform);

                    if (TurnLeftUntil(f1, rotationOffset))
                    {
                        print("TurnLeftUntil is done");
                        strafeDirection = StrafeDirection.Unknown;
                        // strafe
                        stage = RobotStage.PickupStrafe;
                    }

                    break;

                case RobotStage.Pickup:
                    if (!pickingup)
                    {
                        print("start pickup");
                        pickingup = true;
                        // disable dog animator, otherwise arm animator won't work.
                        dogAnimator.enabled = false;
                        armAnimator.enabled = true;

                        RPC_UpdateAnimatorStatus(dogAnimator.enabled, armAnimator.enabled);

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

                            RPC_UpdateAnimatorStatus(dogAnimator.enabled, armAnimator.enabled);

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

                        RPC_UpdateAnimatorStatus(dogAnimator.enabled, armAnimator.enabled);

                        StartDropoffAnimation();
                    }
                    else
                    {
                        if (IsDropoffDone())
                            // if (dropoffDone)
                        {
                            print("dropoff done");
                            droppingoff = false;
                            // reset arm
                            roboticArm.ResetRotations();

                            dogAnimator.enabled = true;
                            armAnimator.enabled = false;

                            RPC_UpdateAnimatorStatus(dogAnimator.enabled, armAnimator.enabled);

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
            var pos1 = body.position;
            var pos2 = targetTransform.position;

            pos1.y = 0;
            pos2.y = 0;

            var robotForward = body.forward;

            var angle = Vector3.SignedAngle(robotForward, pos2 - pos1, Vector3.up);
            return angle;
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

            // update global constant
            GlobalConstants.lastSpawnedPipe = go;

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


        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_UpdateAnimatorStatus(bool dog, bool arm, RpcInfo info = default)
        {
            if (info.IsInvokeLocal)
            {
                print($"Sync animator status: {dog} - {arm}");
            }
            else
            {
                print($"Update animator status: {dog} - {arm}");
                dogAnimator.enabled = dog;
                armAnimator.enabled = arm;
            }
        }

        #endregion
    }
}