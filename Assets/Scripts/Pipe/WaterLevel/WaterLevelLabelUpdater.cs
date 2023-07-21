using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using VRC2.Pipe;
using VRC2.Scenarios;

namespace VRC2.Events
{
    public class WaterLevelLabelUpdater : NetworkBehaviour
    {
        [Header("Label")] public TextMeshPro textMeshPro;

        private bool privilegeUpdated = false;

        private NetworkRunner runner
        {
            get => GlobalConstants.networkRunner;
        }

        private void Start()
        {

        }

        private void FixedUpdate()
        {
            SyncWaterLevel();
        }

        void SyncWaterLevel()
        {
            var d = GetDegree();
            textMeshPro.text = $"{d}";

            if (runner != null && runner.IsRunning && runner.IsClient)
            {
                // P2 side
                var t = gameObject.transform;

                RPC_SendMessage(t.position, t.rotation, d);
            }
        }

        int GetDegree()
        {
            var t = gameObject.transform.rotation.eulerAngles;
            return (int)(Math.Abs(t.x % 90));
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(Vector3 pos, Quaternion rot, int deg, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent : {pos} {rot} {deg}\n";
            else
            {
                if (!privilegeUpdated)
                {
                    // disable interaction
                    PipeHelper.DisableInteraction(gameObject);
                    privilegeUpdated = true;
                }

                message = $"Some other said : {pos} {rot} {deg}\n";
                gameObject.transform.position = pos;
                gameObject.transform.rotation = rot;
                textMeshPro.text = $"{deg}";
            }

            Debug.LogWarning(message);
        }
    }
}