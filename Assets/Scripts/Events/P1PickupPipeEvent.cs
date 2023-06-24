using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class P1PickupPipeEvent : BaseEvent
    {
        public NetworkPrefabRef prefab;


        #region Synchronize material and size

        // NOTE: here variables and methods should be static except `spawned`
        // refer: https://doc.photonengine.com/zh-cn/fusion/current/tutorials/host-mode-basics/5-property-changes

        [HideInInspector] public static PipeMaterialColor pipeColor;
        [HideInInspector] public static float pipeSize;
        private static NetworkObject spawnedPipe;

        [Networked(OnChanged = nameof(OnPipeSpawned))]
        public NetworkBool spawned { get; set; }

        static void OnPipeSpawned(Changed<P1PickupPipeEvent> changed)
        {
            // update locally
            UpdateLocalSpawnedPipe();
        }

        static void UpdateLocalSpawnedPipe()
        {
            var go = spawnedPipe.gameObject;
            var pm = go.GetComponent<PipeManipulation>();
            
            // update color and size
            pm.SetMaterial(pipeColor);
            pm.SetSize(pipeSize);
            
            // set no showing label
            var plc = go.GetComponent<PipeLabelController>();
            plc.showWhenHover = false;
            
            // set it to not spawnable
            var pg = go.GetComponent<PipeGrabbable>();
            pg.isSpawnedPipe = true;
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public override void Execute()
        {
            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }
            
            SpawnPipeUsingTemplate();
            
            // send message
            RPC_SendMessage(spawnedPipe.Id, pipeColor, pipeSize);
        }

        internal void SpawnPipeUsingTemplate()
        {
            var template = GlobalConstants.pipeSpawnTemplate;
            var t = template.transform;
            var pos = t.position;
            var rot = t.rotation;
            // var scale = t.localScale;
            
            // make it a bit closer to the camera
            var offset = -Camera.main.transform.forward;
            pos += offset * 0.1f;
            
            // update static variables
            var pm = template.GetComponent<PipeManipulation>();
            P1PickupPipeEvent.pipeColor = pm.pipeColor;
            P1PickupPipeEvent.pipeSize = pm.pipeSize;
            
            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;
            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);
            spawned = !spawned;
        }


        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeMaterialColor color, float size, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"P1PickupPipeEvent message ({nid}, {color}, {size})\n";
            else
            {
                message = $"P1PickupPipeEvent received message ({nid}, {color}, {size})\n";
                // update spawned object material
            }

            Debug.LogWarning(message);
        }
    }
}