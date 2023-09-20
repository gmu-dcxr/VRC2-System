using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncer : NetworkBehaviour
    {
        public void Synchronize(int pid, bool male)
        {
            // if (Runner != null && Runner.isActiveAndEnabled)
            // {
            //     RPC_SendMessage(pid, male);
            // }
            // else
            // {
            //     print("Runner is not valid");
            // }
            RPC_SendMessage(pid, male);
        }

        PlayerRef GetPlayerByPID(int pid)
        {
            var players = Runner.ActivePlayers;
            foreach (var p in players)
            {
                if (p.PlayerId == pid) return p;
            }

            return PlayerRef.None;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(int playerid, bool male, RpcInfo info = default)
        {
            if (info.IsInvokeLocal)
            {
                print("local invoke");
            }
            else
            {
                print("remote invoke");
            }
            print($"GenderSyncer: {playerid} {male}");
            // find gameobject by player id
            var pr = GetPlayerByPID(playerid);
            var go = Runner.GetPlayerObject(pr);

            print($"network object id: {go.Id}");

            // get GenderSelector
            var gs = go.GetComponent<GenderSelector>();

            if (male)
            {
                gs.ChangeToMale();
            }
            else
            {
                gs.ChangeToFemale();
            }
        }
    }
}