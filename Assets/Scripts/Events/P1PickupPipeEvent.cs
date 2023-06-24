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
        [HideInInspector] public static int pipeSize;
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

            if (pm != null)
            {
                pm.SetMaterial(pipeColor);
                pm.SetSize(pipeSize);
            }
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

            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;
            spawnedPipe = runner.Spawn(prefab, new Vector3(0f, 1.5f, 2f), Quaternion.identity, localPlayer);
            spawned = !spawned;
            // send message
            RPC_SendMessage(spawnedPipe.Id, pipeColor, pipeSize);
        }


        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeMaterialColor color, int size, RpcInfo info = default)
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