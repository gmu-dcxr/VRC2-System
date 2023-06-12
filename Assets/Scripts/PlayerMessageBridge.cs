using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace VRC2
{
    public class PlayerMessageBridge : NetworkBehaviour
    {

        public System.Action<string> OnMessageSent;

        public System.Action<string> OnMessageReceived;

        // public method wrapper
        public void Send(string message)
        {
            RPC_SendMessage(message);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_SendMessage(string message, RpcInfo info = default)
        {
            if (info.IsInvokeLocal)
            {
                // sender side
                if (OnMessageSent != null)
                {
                    OnMessageSent(message);
                }
            }
            else
            {
                // receiver side
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(message);
                }
            }
        }
    }
}