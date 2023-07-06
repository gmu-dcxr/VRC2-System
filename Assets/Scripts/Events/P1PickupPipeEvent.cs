using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;

namespace VRC2.Events
{
    public class P1PickupPipeEvent : BaseEvent
    {
        #region Synchronize material and size

        // NOTE: here variables and methods should be static except `spawned`
        // refer: https://doc.photonengine.com/zh-cn/fusion/current/tutorials/host-mode-basics/5-property-changes

        [HideInInspector] public static PipeMaterialColor pipeColor;
        [HideInInspector] public static float pipeLength;
        private static NetworkObject spawnedPipe;

        [Networked(OnChanged = nameof(OnPipeSpawned))]
        [HideInInspector]
        public NetworkBool spawned { get; set; }

        static void OnPipeSpawned(Changed<P1PickupPipeEvent> changed)
        {
            try
            {
                // update locally
                UpdateLocalSpawnedPipe();
            }
            catch (Exception e)
            {
                // remote client also called this function
                Debug.LogException(e);
            }
        }

        static void UpdateLocalSpawnedPipe()
        {
            var go = spawnedPipe.gameObject;
            var pm = go.GetComponent<PipeManipulation>();

            // update color and size
            pm.SetMaterial(pipeColor);
            pm.SetLength(pipeLength);
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
            
            // update global constant
            GlobalConstants.lastSpawned = spawnedPipe.Id;

            // send message
            RPC_SendMessage(spawnedPipe.Id, pipeColor, pipeLength);
        }

        internal void SpawnPipeUsingTemplate()
        {
            var template = GlobalConstants.pipeSpawnTemplate;
            var t = template.transform;
            var pos = t.position;
            var rot = t.rotation;
            // var scale = t.localScale;
            
            var pm = template.GetComponent<PipeManipulation>();
            var color = pm.pipeColor;
            var length = pm.pipeLength;
            var diameter = pm.diameter;
            
            // destroy
            GameObject.DestroyImmediate(GlobalConstants.pipeSpawnTemplate);
            GlobalConstants.pipeSpawnTemplate = null;
            
            // update static variables
            pipeColor = color;
            pipeLength = length;

            // make it a bit closer to the camera
            var offset = -Camera.main.transform.forward;
            pos += offset * 0.1f;

            // spawn object
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;
            
            // get prefab by diameter
            var prefab = GlobalConstants.GetPipeNetworkPrefabRef(diameter);
            
            spawnedPipe = runner.Spawn(prefab, pos, rot, localPlayer);
            spawned = !spawned;
        }

        internal static void SetSpawnedPipeNotSpawnable(GameObject go)
        {
            // set no showing label
            var plc = go.GetComponent<PipeLabelController>();
            plc.showWhenHover = false;
        }

        // update spawned pipe since it might be different from the prefab
        void UpdateRemoteSpawnedPipe(NetworkId nid, PipeMaterialColor color, float size)
        {
            var runner = GlobalConstants.networkRunner;
            var go = runner.FindObject(nid).gameObject;

            // update material and size
            var pm = go.GetComponent<PipeManipulation>();
            pm.SetMaterial(color);
            pm.SetLength(size);

            SetSpawnedPipeNotSpawnable(go);
        }


        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(NetworkId nid, PipeMaterialColor color, float size, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                message = $"P1PickupPipeEvent message ({nid}, {color}, {size})\n";
                Debug.LogWarning(message);   
            }
            else
            {
                message = $"P1PickupPipeEvent received message ({nid}, {color}, {size})\n";
                Debug.LogWarning(message);
                
                // update spawned object material
                UpdateRemoteSpawnedPipe(nid, color, size);
            }
        }
    }
}