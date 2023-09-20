using System;
using Fusion;
using UnityEngine;

using Timer = UnityTimer.Timer;

namespace VRC2.Character
{
    public class GenderSyncHostToClient : NetworkBehaviour
    {
        private PlayerSpawner _playerSpawner;

        private bool synchronized = false;

        private bool timerStarted = false;
        private Timer _timer;

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
            timerStarted = false;
        }

        // Note: The reason why we need a timer here is that
        // 1) Fusion network process in the following order:
        //  a) Host: host in the scene
        //  b) Join: joiner joins the scene, the scene in the host end will have two clones first, and the scene in
        //      the joiner end may not have two clones. 
        // 2) It won't work in the remote end if we directly call rpc without waiting.
        // 3) Joiner to host is different because when the joiner side has two clones, the remote side (host) must have
        //      already two clones.
        void SetTimer(Action complete)
        {
            timerStarted = true;

            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(5.0f, complete, isLooped: false, useRealTime: true);
        }

        private void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender() && Runner.IsServer)
            {
                if (!timerStarted)
                {
                    SetTimer(() =>
                    {
                        var pid = GlobalConstants.localPlayer.PlayerId;
                        var male = GlobalConstants.playerGender == PlayerGender.Male;
                        // send message
                        RPC_SendMessage(pid, male);
                        synchronized = true;
                    });
                }
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
            // find gameobject by player id
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