using System;
using Fusion;
using UnityEngine;
using VRC2.Events;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeBendCutParameters = VRC2.Pipe.PipeConstants.PipeBendCutParameters;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;

namespace VRC2
{
    public class RobotBendCut : BaseEvent
    {
        [Header("Robot Setting")] public GameObject robot;

        public Transform startPoint;
        public Transform middlePoint;

        private PipeBendCutParameters _parameters;

        private GameObject _currentPipe;

        [Header("Prefab")] public NetworkPrefabRef prefab;

        #region Synchronize Remote Object

        [HideInInspector] public static PipeBendCutParameters pipeParameters;


        private static NetworkObject spawnedPipe;

        private static GameObject staticRobot;
        private static Transform staticStartPoint;

        [Networked(OnChanged = nameof(OnPipeSpawned))]
        [HideInInspector]
        public NetworkBool spawned { get; set; }

        static void OnPipeSpawned(Changed<RobotBendCut> changed)
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

            // enable only
            pm.EnableOnly(pipeParameters.angle);
            // set material
            pm.SetMaterial(pipeParameters.color);
            // edit mesh
            EditMesh(go, pipeParameters.angle, pipeParameters.a, pipeParameters.b);

            // deliver the pipe to the start point
            staticRobot.transform.position = staticStartPoint.position;
            // un parent
            spawnedPipe.transform.parent = null;
        }

        // update spawned pipe since it might be different from the prefab
        void UpdateRemoteSpawnedPipe(NetworkId nid, PipeMaterialColor color, PipeBendAngles angle, float a, float b)
        {
            var runner = GlobalConstants.networkRunner;
            var go = runner.FindObject(nid).gameObject;

            // update material
            var pm = go.GetComponent<PipeManipulation>();

            // enable only
            pm.EnableOnly(angle);
            // set material
            pm.SetMaterial(color);
            // edit mesh
            EditMesh(go, angle, a, b);
        }

        internal void SpawnPipeUsingSelected()
        {
            var template = GlobalConstants.selectedPipe;
            var t = template.transform;
            var pos = t.position;
            var rot = t.rotation;
            // var scale = t.localScale;


            // destroy
            GameObject.DestroyImmediate(GlobalConstants.selectedPipe);
            GlobalConstants.selectedPipe = null;

            // // make it a bit closer to the camera
            // var offset = -Camera.main.transform.forward;
            // pos += offset * 0.1f;

            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);
            spawned = !spawned;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeMaterialColor color, PipeBendAngles angle, float a, float b,
            RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"RobotBendCut message ({color}, {angle}, {a}, {b})\n";
                Debug.LogWarning(message);
            }
            else
            {
                message = $"RobotBendCut received message ({color}, {angle}, {a}, {b})\n";
                Debug.LogWarning(message);

                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, color, angle, a, b);
            }
        }

        #endregion


        private void Start()
        {
            staticStartPoint = startPoint;
            staticRobot = robot;
        }

        public void InitParameters(PipeBendAngles angle, float a, float b)
        {
            _parameters.angle = angle;
            _parameters.a = a;
            _parameters.b = b;
            // color and type and from global constants
            _currentPipe = GlobalConstants.selectedPipe;
            // get color and type
            var pm = _currentPipe.GetComponent<PipeManipulation>();
            _parameters.color = pm.pipeColor;
            _parameters.type = pm.pipeType;
            _parameters.diameter = pm.diameter;

            // update static variables
            pipeParameters.color = _parameters.color;
            pipeParameters.angle = _parameters.angle;
            pipeParameters.a = _parameters.a;
            pipeParameters.b = _parameters.b;
            pipeParameters.type = _parameters.type;
            pipeParameters.diameter = _parameters.diameter;
        }

        public void PickUp()
        {
            Debug.Log("Robot PickUp");
            // get the pipe from P1, and move to the middle point where to bend/cut

            var pipe = GlobalConstants.selectedPipe;
            // set parent to robot
            pipe.transform.parent = robot.transform.parent;
            // move robot to the middle point to do bend or cut
            robot.transform.position = middlePoint.position;
        }

        public void Execute()
        {
            PickUp();
            // bend/cut using spawned pipe
            SpawnPipeUsingSelected();

            RPC_SendMessage(spawnedPipe.Id, pipeParameters.color, pipeParameters.angle, pipeParameters.a,
                pipeParameters.b);
        }

        static void EditMesh(GameObject go, PipeBendAngles angle, float a, float b)
        {
            // TODO: 
            var pm = go.GetComponent<PipeManipulation>();

            switch (angle)
            {
                case PipeBendAngles.Angle_0:
                    pm.SimulateStraightCut(a);
                    break;
                default:
                    break;
            }
        }

    }
}