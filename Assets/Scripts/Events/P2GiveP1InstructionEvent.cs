using UnityEngine;
using Fusion;

namespace VRC2.Events
{
    public class P2GiveP1InstructionEvent : BaseEvent
    {
        // TODO: devise a window
        public float size = 1.0f;
        public Color color = Color.green;

        public override void Execute()
        {
            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            RPC_SendMessage(size, color);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(float size, Color color, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent instruction: size {size} and color {color.ToString()}\n";
            else
            {
                message = $"Some other player said instruction: size {size} and color {color.ToString()}\n";
                // show check result window
                modalDialog.UpdateDialog("Instruction", $"Pipe size {size} and color {color}", "OK", null,
                    PipeInstallEvent.P1GetInstruction);
                modalDialog.show(true);
            }

            Debug.LogWarning(message);
        }
    }
}