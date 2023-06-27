using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

namespace VRC2.Events
{

    public class P2CheckSizeAndColorEvent : BaseEvent
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
                message = $"Some other player said: {check}\n";
                // show check result window
                dialogManager.UpdateDialog("Check Result", $"Pipe color and size check result: {check}", "OK", null,
                    PipeInstallEvent.P1GetSizeAndColorResult);
                dialogManager.checkResult = check;
                dialogManager.Show(true);
            }

            Debug.LogWarning(message);
        }

        #region Get Size And Color

        public static (float, PipeMaterialColor) GetPipeSizeAndColor()
        {
            float size = -1.0f;
            PipeMaterialColor color = PipeMaterialColor.Default;

            if (GlobalConstants.lastSpawned.IsValid)
            {
                var runner = GlobalConstants.networkRunner;
                var obj = runner.FindObject(GlobalConstants.lastSpawned);
                // interactable pipe
                var go = obj.gameObject;
                // pipe manipulation
                var pm = go.GetComponent<PipeManipulation>();
                size = pm.pipeSize;
                color = pm.pipeColor;
            }

            return (size, color);
        }

        #endregion
    }
}
