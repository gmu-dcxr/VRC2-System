using System;
using UnityEngine;

using System;
using Fusion;
using Oculus.Interaction.DistanceReticles;
using UnityEngine;
using UnityEngine.AI;
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
        PickupPrepare = 4,
        Pickup = 5,
        Dropoff = 6,
    }

    public class RobotDogController : MonoBehaviour
    {
        public GameObject robotDog;
        // public GameObject pipe;

        public GameObject attachePoint;

        public RobotDogInputRecording recording;
        public RobotDogInputReplay replay;

        private RobotStage stage;

        private float angleThreshold = 10f;
        private float distanceThreshold = 0.5f;

        private bool pickingup = false;
        private bool droppingoff = false;

        private GameObject targetGameObject;

        #region Targets

        [Space(30)]
        [Header("Target")]
        public Transform bendcutInput;
        public Transform bendcutOutput;
        public Transform deliveryPoint;
        public Transform standbyPoint;
        
        private Transform targetTransform;

        [Space(30)] [Header("Arm")] public RoboticArm roboticArm;

        #endregion

        #region For network

        [HideInInspector] public GameObject currentPipe { get; set; }

        private PipeParameters parameters;

        private NetworkObject spawnedPipe;

        public System.Action<PipeBendAngles> ReadyToOperate;

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

            // set arm reference
            replay.arm = recording.arm;
            replay.recording = recording;

            recording.OnCloseGripOnce += OnCloseGripOnce;
            recording.OnNeedReleasingOnce += OnNeedReleasingOnce;
            
            roboticArm.ReadyToPickup += ReadyToPickup;

        }

        private void ReadyToPickup()
        {
            print("ReadyToPickup");
            var pipe = GlobalConstants.lastSpawnedPipe;
            
            // remove rigid body
            PipeHelper.BeforeMove(ref pipe);
            // update box colliders
            PipeHelper.UpdateBoxColliders(pipe, false);
            
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

        // Tip: better to use FixedUpdate than Update for animation replaying
        public void FixedUpdate()
        {
            if (stage == RobotStage.Default) return;

            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Forward:
                    var distance = GetDistance(targetTransform);

                    ForceRobotTowards(targetTransform);

                    if (distance < distanceThreshold)
                    {
                        replay.Forward(false, true);
                        // stage = RobotStage.Stop;
                        // force update position
                        // ForceRobotPosition(target);

                        if (currentPipe != null && targetTransform == currentPipe.transform)
                        {
                            // pickup
                            // make it ready for pickup
                            stage = RobotStage.PickupPrepare;
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
                            stage = RobotStage.PickupPrepare;
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
                    else
                    {
                        replay.Forward(true);
                    }

                    break;

                case RobotStage.Left:
                    var angle = Math.Abs(GetForwardAngleDiff());
                    // print($"left: {angle}");
                    if (angle < angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.LeftTurn(false, true);
                        // ForceRobotTowards(targetTransform);
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        replay.LeftTurn(true, false);
                    }

                    break;
                case RobotStage.Right:
                    angle = Math.Abs(GetForwardAngleDiff());
                    // print($"right: {angle}");
                    if (angle < angleThreshold)
                    {
                        // stop and force updating the rotation
                        replay.RightTurn(false, true);
                        // ForceRobotTowards(targetTransform);
                        stage = RobotStage.Forward;
                    }
                    else
                    {
                        replay.RightTurn(true, false);
                    }

                    break;

                case RobotStage.PickupPrepare:
                    // var f1 = targetTransform.forward;
                    var f1 = GetCompensateForward(targetGameObject.transform);
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
                            print("dropoff done");
                            pickingup = false;
                            // reset arm
                            recording.ResetArm();

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
            //     stage = RobotStage.PickupPrepare;
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
        }

        #endregion

        private void Update()
        {
            
        }

        #region Animation monitor

        void MonitorAnimation()
        {
            
        }

        

        #endregion
    }
}