using System;
using System.Collections;
using Fusion;
using UnityEngine;

using Timer = UnityTimer.Timer;

namespace VRC2.Character
{
    public class GenderSyncHostToClient : GenderSyncBase
    {
        private Timer _timer;

        public void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            synchronized = false;
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
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(5.0f, complete, isLooped: false, useRealTime: true);
        }

        public void Update()
        {
            if (synchronized) return;

            if (_playerSpawner.ReadyToSyncGender() && Runner.IsServer)
            {
                synchronized = true;
                StartCoroutine(SendMessageAfter(5.0f));

                // SetTimer(() =>
                // {
                //     var pid = GlobalConstants.localPlayer.PlayerId;
                //     var female = GlobalConstants.playerGender == PlayerGender.Female;
                //     var hair = GlobalConstants.playerHairIndex;
                //     var skin = GlobalConstants.playerSkinIndex;
                //
                //     print($"GenderSyncHostToClient: {pid} {female} {hair} {skin}");
                //     // send message
                //     RPC_SendMessage(pid, female, hair, skin);
                // });
            }
        }

        IEnumerator SendMessageAfter(float second)
        {
            yield return new WaitForSeconds(second);
            var pid = GlobalConstants.localPlayer.PlayerId;
            var female = GlobalConstants.playerGender == PlayerGender.Female;
            var hair = GlobalConstants.playerHairIndex;
            var skin = GlobalConstants.playerSkinIndex;
            
            print($"GenderSyncHostToClient: {pid} {female} {hair} {skin}");
            // send message
            RPC_SendMessage(pid, female, hair, skin);
            yield break;
        }
    }
}