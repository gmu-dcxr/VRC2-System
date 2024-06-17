using System;
using Fusion;
using UnityEngine;
using FishNetNetworkObject = FishNet.Object.NetworkObject;

namespace VRC2.Character
{
    public class GenderSyncBase : NetworkBehaviour
    {
        [HideInInspector] public PlayerSpawner _playerSpawner;

        [HideInInspector] public bool synchronized = false;

        [HideInInspector] public NetworkRunner runner => GetComponent<NetworkObject>().Runner;
        public PlayerRef GetPlayerByPID(int pid)
        {
            var players = Runner.ActivePlayers;
            foreach (var p in players)
            {
                if (p.PlayerId == pid) return p;
            }

            return PlayerRef.None;
        }

        public GameObject FindSpawnedFishNetObject(int oid)
        {
            var gos = GameObject.FindObjectsOfType<OVRCustomSkeleton>();

            foreach (var go in gos)
            {
                var obj = go.gameObject.GetComponent<FishNetNetworkObject>();
                if (obj.ObjectId == oid) return go.gameObject;
            }

            return null;
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

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage_FishNet(int objectid, bool female, int hair, int skin, RpcInfo info = default)
        {
            var go = FindSpawnedFishNetObject(objectid);
            
            if(go == null) return;
            
            var cms = go.GetComponent<CharacterMaterialSelector>();
            print($"RPC_SendMessage_FishNet: {objectid} {female} {hair} {skin}");

            cms.UpdateAppearance(female, hair, skin);
        }
    }
}