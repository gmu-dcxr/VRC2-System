using System;
using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncClientToHost : GenderSyncBase
    {
        public void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
        }

        public void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender(true))
            {
                synchronized = true;

                // Fusion
                // var pid = GlobalConstants.localPlayer.PlayerId;
                // fishnet
                var pid = GlobalConstants.localFishNetPlayer;
                var female = GlobalConstants.playerGender == PlayerGender.Female;
                var hair = GlobalConstants.playerHairIndex;
                var skin = GlobalConstants.playerSkinIndex;

                print($"GenderSyncClientToHost: {pid} {female} {hair} {skin}");
                // send message
                // RPC_SendMessage(pid, female, hair, skin);
                RPC_SendMessage_FishNet(pid, female, hair, skin);
            }
        }
    }
}