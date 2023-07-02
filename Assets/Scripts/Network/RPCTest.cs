using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace VRC2
{

    public class RPCTest : NetworkBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
            {
                RPC_SendMessage("Hey Mate!");
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(string message, RpcInfo info = default)
        {
            var _messages = "";

            if (info.IsInvokeLocal)
                message = $"You said: {message}\n";
            else
                message = $"Some other player said: {message}\n";
            _messages = message;

            Debug.LogWarning(_messages);
        }
    }
}