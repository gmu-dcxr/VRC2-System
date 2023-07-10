using System;
using Fusion;
using UnityEngine;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeBendCutParameters = VRC2.Pipe.PipeConstants.PipeBendCutParameters;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;


namespace VRC2.Events
{
    public class AIDroneDeliver : BaseEvent
    {
        [Header("AI Drone Setting")] public GameObject drone;

        private AIDroneController _droneController;

        public GameObject droneBase; // where is the drone is
        public GameObject warehouse; // where to get the pipe
        public GameObject storage; // where is the storage place


        [Header("Prefab")] public NetworkPrefabRef prefab;


        private float pipeDroneDistance = 0.2f;

        private PipeBendCutParameters parameters;

        private NetworkObject spawnedPipe;

        void UpdateLocalSpawnedPipe(GameObject go)
        {
            var pm = go.GetComponent<PipeManipulation>();

            // enable straight pipe
            pm.EnableOnly(PipeBendAngles.Angle_0);
            // set material
            pm.SetMaterial(parameters.color);
        }

        // update spawned pipe since it might be different from the prefab
        void UpdateRemoteSpawnedPipe(NetworkId nid, PipeType type, PipeMaterialColor color)
        {
            var runner = GlobalConstants.networkRunner;
            var go = runner.FindObject(nid).gameObject;

            // update material
            var pm = go.GetComponent<PipeManipulation>();

            // enable only
            pm.EnableOnly(PipeBendAngles.Angle_0);
            // set material
            pm.SetMaterial(color);
        }

        void SpawnPipeUsingSelected(PipeType type, PipeDiameter diameter)
        {
            // TODO: spawn different pipe according to different pipes

            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            var pos = warehouse.transform.position;
            var rot = Quaternion.identity;

            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);

            UpdateLocalSpawnedPipe(spawnedPipe.gameObject);

            RPC_SendMessage(spawnedPipe.Id, parameters.type, parameters.color);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeType type, PipeMaterialColor color, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"AIDrone message ({type}, {color})\n";
                Debug.LogWarning(message);
                // start pick up
                _droneController.PickUp();
            }
            else
            {
                message = $"RobotBendCut received message ({type}, {color})\n";
                Debug.LogWarning(message);

                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, type, color);
            }
        }

        void Start()
        {
            _droneController = drone.GetComponent<AIDroneController>();

            _droneController.pipeWarehouse = warehouse;
            _droneController.pipeStorage = storage;
            _droneController.droneBase = droneBase;

            _droneController.ReadyToPickUp += ReadyToPickUp;
            _droneController.ReadyToDropOff += ReadyToDropOff;
            _droneController.ReadyToReturnToBase += ReadyToReturnToBase;
        }

        private void ReadyToReturnToBase()
        {
            print("ReadyToReturnToBase");
            _droneController.Stop();
        }

        private void ReadyToDropOff()
        {
            print("ReadyToDropoff");
            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;
            // spawn a new pipe
            var t = spawnedPipe.transform;
            var newSpawned = runner.Spawn(prefab, t.position, t.rotation, localPlayer);
            // update it
            UpdateLocalSpawnedPipe(newSpawned.gameObject);

            // Despawn the previous one
            spawnedPipe.transform.parent = null;
            runner.Despawn(spawnedPipe);

            spawnedPipe = newSpawned;

            _droneController.ReturnToBase();
        }

        private void ReadyToPickUp()
        {
            print("ReadyToDropOff");
            // set pipe's parent to drone
            spawnedPipe.transform.parent = _droneController.gameObject.transform;

            // remove its rigid body. This drone can not have workload right now.
            var go = spawnedPipe.gameObject;
            var rb = go.GetComponent<Rigidbody>();
            GameObject.Destroy(rb);

            var pos = _droneController.gameObject.transform.position;
            pos.y -= pipeDroneDistance;

            spawnedPipe.transform.position = pos;
            spawnedPipe.transform.localRotation = Quaternion.identity;

            _droneController.DropOff();
        }

        public override void Execute()
        {
            var type = parameters.type;
            var color = parameters.color;
            var diameter = parameters.diameter;
            SpawnPipeUsingSelected(type, diameter);
        }

        public void InitParameters(PipeBendCutParameters para)
        {
            // update static variables
            parameters.type = para.type;
            parameters.color = para.color;
            parameters.diameter = para.diameter;
        }
    }
}