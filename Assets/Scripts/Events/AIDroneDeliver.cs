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

        private PipeBendCutParameters _parameters;

        private GameObject _currentPipe;

        [Header("Prefab")] public NetworkPrefabRef prefab;


        private NetworkObject spawnedPipe;

        void Start()
        {
        }

        public override void Execute()
        {
            MoveToWarehouse();
            PickupPipe();
            MoveToStorage();
            DropoffPipe();
            BackToStart();
        }

        public void InitParameters(PipeBendCutParameters parameters)
        {
            _parameters.type = parameters.type;
            _parameters.color = parameters.color;
            _parameters.diameter = parameters.diameter;
            // other parameters are unnecessary
        }

        public void MoveToWarehouse()
        {
            // backup current position
            startPoint = drone.transform.position;

            Debug.Log("[AI Drone] backup the position");

            // move to ware house
            drone.transform.position = warehousePoint;

            Debug.Log("[AI Drone] move to warehouse");

        }

        public void MoveToStorage()
        {
            Debug.Log("[AI Drone] move to storage");
            drone.transform.position = storagePoint;
        }

        public void DropoffPipe()
        {
            Debug.Log("[AI Drone] drop off pipe");
            spawnedPipe.transform.parent = null;
        }

        public void BackToStart()
        {
            Debug.Log("[AI Drone] back to start point");
            drone.transform.position = startPoint;
        }

        public void PickupPipe()
        {
            Debug.Log("[AI Drone] pickup a pipe");

            // use current parameters to spawn a pipe
            var type = _parameters.type;
            var color = _parameters.color;
            var diameter = _parameters.diameter;

            // initialize different pipe use the parameters
            var pipe = SpawnPipe(type, diameter);

            // update the pipe appearance
            // update material
            var pm = pipe.gameObject.GetComponent<PipeManipulation>();

            // enable straight pipe (default setting)
            pm.EnableOnly(PipeBendAngles.Angle_0);
            // set material
            pm.SetMaterial(color);

            // update pipe's parent
            spawnedPipe.transform.parent = drone.transform;
        }

        public NetworkObject SpawnPipe(PipeType type, PipeDiameter diameter)
        {
            // TODO: spawn different pipe according to different pipes

            var pos = storagePoint;
            var rot = Quaternion.identity;
            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);

            return spawnedPipe;
        }
    }
}