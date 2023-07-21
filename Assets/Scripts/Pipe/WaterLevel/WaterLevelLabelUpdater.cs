using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    public class WaterLevelLabelUpdater : NetworkBehaviour
    {
        [Header("Label")] public TextMeshPro textMeshPro;


        private NetworkRunner runner
        {
            get => GlobalConstants.networkRunner;
        }

        private void Start()
        {

        }

        private void FixedUpdateNetwork()
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
                message = $"Some other said : {pos} {rot} {deg}\n";
                gameObject.transform.position = pos;
                gameObject.transform.rotation = rot;
                textMeshPro.text = $"{deg}";
            }

            Debug.LogWarning(message);
        }
    }
}