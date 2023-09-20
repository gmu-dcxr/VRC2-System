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

        void SetTimer(Action complete)
        {
            timerStarted = true;

            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(5.0f, complete, isLooped: false, useRealTime: true);
        }

        private GameObject selfGameObject
        {
            get => _playerSpawner.GetSelfObject();
        }

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
            timerStarted = false;
        }

        private void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender() && Runner.IsServer)
            {
                if (!timerStarted)
                {
                    print("start timer");
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
            if (info.IsInvokeLocal)
            {
                print("GenderSyncHostToClient local invoke");
            }
            else
            {
                print("GenderSyncHostToClient remote invoke");
            }

            print($"GenderSyncHostToClient GenderSyncer: {playerid} {male}");
            // find gameobject by player id
            var pr = GetPlayerByPID(playerid);
            var go = Runner.GetPlayerObject(pr);

            print($"GenderSyncHostToClient network object id: {go.Id}");

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