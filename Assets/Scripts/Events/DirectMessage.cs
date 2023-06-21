using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Events;

public class DirectMessage : BaseEvent
{
    private string _title;
    private string _content;

    public string title
    {
        get => _title;
        set => _title = value;
    }

    public string content
    {
        get => _content;
        set => _content = value;
    }
    

    public override void Execute()
    {
        RPC_SendMessage(title, content);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_SendMessage(string t, string m, RpcInfo info = default)
    {
        var message = "";

        if (info.IsInvokeLocal)
            message = $"You sent message [{t}]: {m}\n";
        else
        {
            message = $"You received message [{t}]: {m}\n";
            // show check result window
            dialogManager.UpdateDialog(title, content, "Yes", null, PipeInstallEvent.EmptyEvent);
            dialogManager.Show(true);
        }

        Debug.LogWarning(message);
    }
}
