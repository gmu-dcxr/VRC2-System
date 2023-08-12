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

namespace VRC2
{
    enum RobotRoutine
    {
        Default = 0,
        PickUp = 1,
        BendCut = 2,
        DropOff = 3,

        // waiting for operation
        Waiting = 4,
    }

    public class RobotBendCut : BaseEvent
    {
        [Header("Robot Setting")] public GameObject robot;
        public GameObject robotBase;
        public GameObject robotHand;

        [Header("Grab Offset")] public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;

        private GameObject currentPipe
        {
            get => GlobalConstants.lastSpawnedPipe;
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


        private void Start()
        {
            _routine = RobotRoutine.Default;
            _agent = robot.GetComponent<NavMeshAgent>();
            _agent.stoppingDistance = 0.5f;
        }

        private void Update()
        {
            switch (_routine)
            {
                case RobotRoutine.PickUp:
                    PickupHandler();
                    break;
                case RobotRoutine.BendCut:
                    BendCutHandler();
                    break;
                case RobotRoutine.DropOff:
                    DropoffHandler();
                    break;

                case RobotRoutine.Waiting:
                    break;
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
            // get the pipe from P1, and move to the robot base to bend/cut
            // robot move to the pipe position
            var pos = currentPipe.transform.position;

            _routine = RobotRoutine.PickUp;
            _agent.SetDestination(pos);
        }

        public void Execute()
        {
            PickUp();
        }

        public NetworkObject SpawnPipe()
        {
            // start bend/cut
            SpawnPipeUsingSelected();
            UpdateLocalSpawnedPipe(spawnedPipe.gameObject);
            // update remote object
            RPC_SendMessage(spawnedPipe.Id, parameters.type, parameters.color, parameters.a, parameters.b);
            return spawnedPipe;
        }

        public void Deliver()
        {
            var go = spawnedPipe.gameObject;
            PipeHelper.BeforeMove(ref go);

            // parent object to robot hand
            spawnedPipe.transform.parent = robotHand.transform;

            spawnedPipe.transform.localPosition = positionOffset;
            spawnedPipe.transform.localRotation = Quaternion.Euler(rotationOffset);

            // move to workspace
            _routine = RobotRoutine.DropOff;
            _agent.SetDestination(destination);
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
                currentPipe.transform.parent = robotHand.transform;
                // update local position and rotation
                currentPipe.transform.localPosition = positionOffset;
                currentPipe.transform.localRotation = Quaternion.Euler(rotationOffset);

                var go = currentPipe;

                PipeHelper.BeforeMove(ref go);

                // move to robot base
                _routine = RobotRoutine.BendCut;
                _agent.SetDestination(robotBase.transform.position);
            }
        }

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

        void DropoffHandler()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                spawnedPipe.transform.parent = null;

                var go = spawnedPipe.gameObject;
                PipeHelper.AfterMove(ref go);

                // return to base
                _routine = RobotRoutine.Default;
                _agent.SetDestination(robotBase.transform.position);
            }
        }

        #endregion
    }
}