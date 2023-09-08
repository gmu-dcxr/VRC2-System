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

            // set arm reference
            replay.arm = recording.arm;
            replay.recording = recording;
            
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
            if(stage == RobotStage.Default) return;
            
            switch (stage)
            {
                case RobotStage.Stop:
                    replay.Stop(true);
                    break;

                case RobotStage.Forward:
                    var distance = GetDistance(targetTransform);
                    
                    print(distance);
                    
                    ForceRobotTowards(targetTransform);

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
                    print($"right: {angle}");
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
                stage = RobotStage.Default;
                replay.RewindPickup();
                replay.Pickup();
            }
        }

        #region WIP - Adaptation
        
        [Header("Robot Setting")] public GameObject robot;
        public Transform robotBase;
        public Transform robotHand;

        [Header("Grab Offset")] public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;

        private GameObject currentPipe
        {
            // get => GlobalConstants.lastSpawnedPipe;
            get => pipe;
        }

        private PipeParameters parameters;

        private NetworkObject spawnedPipe;

        private NavMeshAgent _agent;

        private RobotRoutine _routine;

        private Vector3 destination;

        public System.Action<PipeBendAngles> ReadyToOperate;
        
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


        // private void Start()
        // {
        //     _routine = RobotRoutine.Default;
        //     _agent = robot.GetComponent<NavMeshAgent>();
        //     _agent.stoppingDistance = 0.5f;
        // }

        // private void Update()
        // {
        //     switch (_routine)
        //     {
        //         case RobotRoutine.PickUp:
        //             PickupHandler();
        //             break;
        //         case RobotRoutine.BendCut:
        //             BendCutHandler();
        //             break;
        //         case RobotRoutine.GetResult:
        //             GetResultHandler();
        //             break;
        //         case RobotRoutine.Waiting:
        //             break;
        //
        //         case RobotRoutine.DropOff:
        //             DropoffHandler();
        //             break;
        //     }
        // }

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
            // get the pipe from P1, and move to pickup the pipe
            
            
            targetTransform = currentPipe.transform;
            
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

        public void PickupResult(Vector3 des)
        {
            // move to pick result
            _routine = RobotRoutine.GetResult;
            _agent.SetDestination(des);
        }

        #region Handlers

        void PickupHandler()
        {
            // reach to the workspace
            if (AgentHelper.ReachDestination(_agent))
            {
                // save for future delivery
                destination = currentPipe.transform.position;
                // set pipe parent to robot hand
                currentPipe.transform.parent = robotHand;
                // update local position and rotation
                currentPipe.transform.localPosition = positionOffset;
                currentPipe.transform.localRotation = Quaternion.Euler(rotationOffset);

                var go = currentPipe;

                PipeHelper.BeforeMove(ref go);

                // move to robot base
                _routine = RobotRoutine.BendCut;
                _agent.SetDestination(robotBase.position);
            }
        }

        // TODO: apply when reaching the machine
        void BendCutHandler()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                if (ReadyToOperate != null)
                {
                    ReadyToOperate(parameters.angle);
                }

                _routine = RobotRoutine.Waiting;
            }
        }

        // TODO: when the result carried by the robot dog reaches the delivery point
        void DropoffHandler()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                spawnedPipe.transform.parent = null;

                var go = spawnedPipe.gameObject;
                PipeHelper.AfterMove(ref go);

                // return to base
                _routine = RobotRoutine.Default;
                _agent.SetDestination(robotBase.position);
            }
        }

        // TODO: when the result is available
        void GetResultHandler()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                var go = spawnedPipe.gameObject;
                PipeHelper.BeforeMove(ref go);

                // parent object to robot hand
                spawnedPipe.transform.parent = robotHand;

                spawnedPipe.transform.localPosition = positionOffset;
                spawnedPipe.transform.localRotation = Quaternion.Euler(rotationOffset);

                // move to workspace
                _routine = RobotRoutine.DropOff;
                _agent.SetDestination(destination);
            }
        }

        #endregion

        #endregion
    }
}