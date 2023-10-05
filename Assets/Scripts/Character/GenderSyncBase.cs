using System;
using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncBase : NetworkBehaviour
    {
        [HideInInspector] public PlayerSpawner _playerSpawner;

        [HideInInspector] public bool synchronized = false;

        public PlayerRef GetPlayerByPID(int pid)
        {
            var players = Runner.ActivePlayers;
            foreach (var p in players)
            {
                if (p.PlayerId == pid) return p;
            }

            return PlayerRef.None;
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(int playerid, bool female, int hair, int skin, RpcInfo info = default)
        {
            var pr = GetPlayerByPID(playerid);
            var go = Runner.GetPlayerObject(pr);
            var cms = go.GetComponent<CharacterMaterialSelector>();

            print($"RPC_SendMessage: {playerid} {female} {hair} {skin}");

            cms.UpdateAppearance(female, hair, skin);
        }
    }
}