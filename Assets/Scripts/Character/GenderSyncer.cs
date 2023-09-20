using System;
using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncer : NetworkBehaviour
    {
        private PlayerSpawner _playerSpawner;

        private bool synchronized = false;

        private NetworkObject target;

        private int playerID;
        private bool isMale;

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
        }

        public void RequestSync(int pid, bool male)
        {
            synchronized = false;
            playerID = pid;
            isMale = male;
        }

        bool TryToGetGameObject()
        {
            var pr = GetPlayerByPID(playerID);
            target = Runner.GetPlayerObject(pr);

            return target != null;
        }

        private void Update()
        {
            if (Runner == null || !Runner.isActiveAndEnabled) return;

            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender() && TryToGetGameObject())
            {
                // send message
                RPC_SendMessage(playerID, isMale);
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