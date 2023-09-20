using System;
using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncClientToHost : NetworkBehaviour
    {
        private PlayerSpawner _playerSpawner;

        private bool synchronized = false;

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
        }

        private void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender() && Runner.IsClient)
            {
                var pid = GlobalConstants.localPlayer.PlayerId;
                var male = GlobalConstants.playerGender == PlayerGender.Male;
                // send message
                RPC_SendMessage(pid, male);
                synchronized = true;
            }
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
            var pr = GetPlayerByPID(playerid);
            var go = Runner.GetPlayerObject(pr);
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