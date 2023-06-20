using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class P2CheckLengthAndAngleEvent: BaseEvent
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
                dialogManager.UpdateDialog("Check Result", $"Pipe length and angle check result: {check}", "OK", null,
                    PipeInstallEvent.P1GetLengthAndAngleResult);
                dialogManager.checkResult = check;
                dialogManager.Show(true);
            }

            Debug.LogWarning(message);
        }
    }
}