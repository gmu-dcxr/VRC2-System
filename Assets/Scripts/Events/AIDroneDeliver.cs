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

        public Transform warehouse; // where to get the pipe
        public Transform storage; // where is the storage place

        private Vector3 startPoint;

        private Vector3 warehousePoint
        {
            get => warehouse.position;
        }

        private Vector3 storagePoint
        {
            get => storage.position;
        }

        private GameObject _currentPipe;

        [Header("Prefab")] public NetworkPrefabRef prefab;

        #region Synchronize Remote Object

        [HideInInspector] public static PipeBendCutParameters parameters;


        private static NetworkObject spawnedPipe;

        private static GameObject staticDrone;
        private static Vector3 staticStartPoint;
        private static Vector3 staticStoragePoint;

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

            Debug.Log($"UpdateLocalSpawnedPipe: {parameters.color}");
            // deliver
            MoveToStorage(staticDrone, go, staticStoragePoint);

            // drop off
            DropoffPipe(go);

            // drone return to base
            BackToBase(staticDrone, staticStartPoint);
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

            var pos = storagePoint;
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
        }

        public override void Execute()
        {

            MoveToWarehouse();
            PickupPipe();
            RPC_SendMessage(spawnedPipe.Id, parameters.type, parameters.color);
        }

        public void InitParameters(PipeBendCutParameters para)
        {
            // update static variables
            parameters.type = para.type;
            parameters.color = para.color;
            parameters.diameter = para.diameter;
            // other parameters are unnecessary
            staticStoragePoint = storage.position;
        }

        public void MoveToWarehouse()
        {
            // backup current position
            startPoint = drone.transform.position;
            staticStartPoint = startPoint;

            Debug.Log("[AI Drone] backup the position");

            // move to ware house
            drone.transform.position = warehousePoint;

            Debug.Log("[AI Drone] move to warehouse");

        }

        public static void MoveToStorage(GameObject drone, GameObject pipe, Vector3 storagePoint)
        {
            Debug.Log("[AI Drone] move to storage");
            pipe.transform.parent = drone.transform;
            drone.transform.position = storagePoint;
        }

        public static void DropoffPipe(GameObject pipe)
        {
            Debug.Log("[AI Drone] drop off pipe");
            pipe.transform.parent = null;
        }

        public static void BackToBase(GameObject drone, Vector3 startPoint)
        {
            Debug.Log("[AI Drone] back to start point");
            drone.transform.position = startPoint;
        }

        public void PickupPipe()
        {
            Debug.Log("[AI Drone] pickup a pipe");

            // use current parameters to spawn a pipe
            var type = parameters.type;
            var color = parameters.color;
            var diameter = parameters.diameter;

            // spawn pipe
            SpawnPipeUsingSelected(type, diameter);
        }
    }
}