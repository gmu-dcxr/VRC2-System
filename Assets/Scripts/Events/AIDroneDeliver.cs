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


        private GameObject _currentPipe;

        [Header("Prefab")] public NetworkPrefabRef prefab;

        #region Synchronize Remote Object

        [HideInInspector] public static PipeBendCutParameters parameters;
        public static AIDroneController staticDroneController;


        private static NetworkObject spawnedPipe;

        private static GameObject staticDrone;

        [Networked(OnChanged = nameof(OnPipePicked))]
        [HideInInspector]
        public NetworkBool picked { get; set; }

        static void OnPipePicked(Changed<AIDroneDeliver> changed)
        {
            try
            {
                // update locally
                UpdateLocalSpawnedPipe();
            }
            catch (Exception e)
            {
                // remote client also called this function
            }
        }

        static void UpdateLocalSpawnedPipe()
        {
            var go = spawnedPipe.gameObject;
            var pm = go.GetComponent<PipeManipulation>();

            // enable straight pipe
            pm.EnableOnly(PipeBendAngles.Angle_0);
            // set material
            pm.SetMaterial(parameters.color);

            // start pick up
            staticDroneController.PickUp();
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

        internal void SpawnPipeUsingSelected(PipeType type, PipeDiameter diameter)
        {
            // TODO: spawn different pipe according to different pipes

            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            var pos = warehouse.transform.position;
            var rot = Quaternion.identity;

            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);
            picked = !picked;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeType type, PipeMaterialColor color, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"AIDrone message ({type}, {color})\n";
                Debug.LogWarning(message);
            }
            else
            {
                message = $"RobotBendCut received message ({type}, {color})\n";
                Debug.LogWarning(message);

                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, type, color);
            }
        }

        #endregion

        void Start()
        {
            _droneController = drone.GetComponent<AIDroneController>();

            _droneController.pipeWarehouse = warehouse;
            _droneController.pipeStorage = storage;
            _droneController.droneBase = droneBase;

            _droneController.ReadyToPickUp += ReadyToPickUp;
            _droneController.ReadyToDropOff += ReadyToDropOff;
            _droneController.ReadyToReturnToBase += ReadyToReturnToBase;

            staticDroneController = _droneController;
        }

        private void ReadyToReturnToBase()
        {
            print("ReadyToReturnToBase");
            _droneController.Stop();
        }

        private void ReadyToDropOff()
        {
            print("ReadyToDropoff");
            spawnedPipe.transform.parent = null;
            _droneController.ReturnToBase();
        }

        private void ReadyToPickUp()
        {
            print("ReadyToDropOff");
            // set pipe's parent to drone
            spawnedPipe.transform.parent = _droneController.gameObject.transform;
            _droneController.DropOff();
        }

        public override void Execute()
        {
            var type = parameters.type;
            var color = parameters.color;
            var diameter = parameters.diameter;
            SpawnPipeUsingSelected(type, diameter);

            RPC_SendMessage(spawnedPipe.Id, parameters.type, parameters.color);
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