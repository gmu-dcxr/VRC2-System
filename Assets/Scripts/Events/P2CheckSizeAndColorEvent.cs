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
                message = $"Some other player said: {message}\n";
                // show check result window
                modalDialog.UpdateDialog("Check Result", $"Pipe color and size check result: {check}", "OK", null,
                    PipeInstallEvent.P1GetSizeAndColorResult);
                modalDialog.checkResult = check;
                modalDialog.show(true);
            }

            Debug.LogWarning(message);
        }
    }
}
