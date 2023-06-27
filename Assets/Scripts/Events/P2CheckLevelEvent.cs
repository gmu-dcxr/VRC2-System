using System;
using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class P2CheckLevelEvent : BaseEvent
    {
        private bool checkPassed;

        public void Initialize(bool checkResult)
        {
            checkPassed = checkResult;
        }

        public override void Execute()
        {
            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            RPC_SendMessage(checkPassed);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(bool check, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent check result: {check}\n";
            else
            {
                message = $"Some other player said: {message}\n";
                // show check result window
                dialogManager.UpdateDialog("Check Result", $"Pipe level check result: {check}", "OK", null,
                    PipeInstallEvent.P1GetLevelResult);
                dialogManager.checkResult = check;
                dialogManager.Show(true);
            }

            Debug.LogWarning(message);
        }
        
        #region Get Water Level
        public static float GetWaterLevelValue()
        {
            float value = -1.0f;
            PipeMaterialColor color = PipeMaterialColor.Default;

            if (GlobalConstants.lastSpawned.IsValid)
            {
                var runner = GlobalConstants.networkRunner;
                var obj = runner.FindObject(GlobalConstants.lastSpawned);
                // interactable pipe
                var go = obj.gameObject;
                // pipe manipulation
                var pm = go.GetComponent<PipeManipulation>();
                var pipe = pm.pipe;
                // get z rotation
                var z = pipe.transform.rotation.eulerAngles.z;
                // normalize
                // TODO: check whether it's reasonable
                value = Math.Abs(z) % 90f;
            }
            
            return value;
        }
        #endregion
    }
}