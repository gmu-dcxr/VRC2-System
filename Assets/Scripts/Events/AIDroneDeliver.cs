using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;
using PipeParameters = VRC2.Pipe.PipeConstants.PipeParameters;


namespace VRC2.Events
{
    public class AIDroneDeliver : BaseEvent
    {
        [Header("AI Drone Setting")] public GameObject drone;

        private AIDroneController _droneController;

        public GameObject droneBase; // where is the drone is
        public GameObject warehouse; // where to get the pipe
        public GameObject storage; // where is the storage place


        private float pipeDroneDistance = 0.2f;

        private PipeConstants.PipeParameters parameters;

        private List<NetworkObject> spawnedPipes = new List<NetworkObject>();

        void UpdateLocalSpawnedPipe(GameObject go)
        {
            var pm = go.GetComponent<PipeManipulation>();

            // set material
            pm.SetMaterial(parameters);
        }

        // update spawned pipe since it might be different from the prefab
        void UpdateRemoteSpawnedPipe(NetworkId nid, PipeParameters para)
        {
            var runner = GlobalConstants.networkRunner;
            var go = runner.FindObject(nid).gameObject;

            // update material
            var pm = go.GetComponent<PipeManipulation>();

            // set material
            pm.SetMaterial(para);
        }

        void SpawnPipeUsingSelected(PipeDiameter diameter, int amount)
        {
            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            var pos = warehouse.transform.position;
            var rot = Quaternion.identity;

            var prefab = PipeHelper.GetStraightPipePrefabRef(diameter);

            spawnedPipes.Clear();

            for (var i = 0; i < amount; i++)
            {
                var pipe = runner.Spawn(prefab, pos, rot, localPlayer);
                GlobalConstants.lastSpawnedPipe = pipe.gameObject;
                UpdateLocalSpawnedPipe(pipe.gameObject);
                RPC_SendMessage(pipe.Id, parameters);

                // add to list
                spawnedPipes.Add(pipe);
            }

            // start pick up
            _droneController.PickUp();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeParameters para, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"AIDrone message ({para.ToString()})\n";
                Debug.LogWarning(message);
                // // start pick up
                // _droneController.PickUp();
            }
            else
            {
                message = $"RobotBendCut received message ({para.ToString()})\n";
                Debug.LogWarning(message);

                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, para);
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

            foreach (var spawnedPipe in spawnedPipes)
            {
                var go = spawnedPipe.gameObject;
                PipeHelper.AfterMove(ref go);
                // Despawn the previous one
                spawnedPipe.transform.parent = null;
            }

            _droneController.ReturnToBase();
        }

        private void ReadyToPickUp()
        {
            print("ReadyToDropOff");

            foreach (var spawnedPipe in spawnedPipes)
            {
                // set pipe's parent to drone
                spawnedPipe.transform.parent = _droneController.gameObject.transform;

                var go = spawnedPipe.gameObject;

                PipeHelper.BeforeMove(ref go);

                var pos = _droneController.gameObject.transform.position;
                pos.y -= pipeDroneDistance;

                spawnedPipe.transform.position = pos;
                spawnedPipe.transform.localRotation = Quaternion.identity;
            }

            _droneController.DropOff();
        }

        public override void Execute()
        {
            var type = parameters.type;
            var color = parameters.color;
            var diameter = parameters.diameter;
            var amount = parameters.amount;
            SpawnPipeUsingSelected(diameter, amount);
        }

        public void InitParameters(PipeParameters para)
        {
            // update static variables
            parameters.type = para.type;
            parameters.color = para.color;
            parameters.diameter = para.diameter;
            // add amount
            parameters.amount = para.amount;
        }
    }
}
