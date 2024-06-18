using System;
using System.Collections;
using Fusion;
using UnityEngine;

namespace VRC2.Character
{
    public class GenderSyncHostToClient : GenderSyncBase
    {
        public void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
        }

        public void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender(false))
            {
                synchronized = true;
                StartCoroutine(SendMessageAfter(5.0f));
            }
        }

        // Note: The reason why we need a wait here is that
        // 1) Fusion network process in the following order:
        //  a) Host: host in the scene
        //  b) Join: joiner joins the scene, the scene in the host end will have two clones first, and the scene in
        //      the joiner end may not have two clones. 
        // 2) It won't work in the remote end if we directly call rpc without waiting.
        // 3) Joiner to host is different because when the joiner side has two clones, the remote side (host) must have
        //      already two clones.
        IEnumerator SendMessageAfter(float second)
        {
            yield return new WaitForSeconds(second);
            // fusion
            // var pid = GlobalConstants.localPlayer.PlayerId;
            // fishnet
            var pid = GlobalConstants.localFishNetPlayer;
            var female = GlobalConstants.playerGender == PlayerGender.Female;
            var hair = GlobalConstants.playerHairIndex;
            var skin = GlobalConstants.playerSkinIndex;

            print($"GenderSyncHostToClient: {pid} {female} {hair} {skin}");
            // send message
            // RPC_SendMessage(pid, female, hair, skin);
            RPC_SendMessage_FishNet(pid, female, hair, skin);
            yield break;
        }
    }
}