using System;
using FishNet.Object;
using FishNet.Transporting.Tugboat;
using UnityEngine;

using FishNetSpawner = FishNet.Component.Spawning.PlayerSpawner;

namespace VRC2.Character
{
    [RequireComponent(typeof(FishNetSpawner))]
    public class FishNetSpawnerHook : MonoBehaviour
    {
        private FishNetSpawner _spawner;

        private Tugboat _tugboat;

        public void Start()
        {
            _spawner = GetComponent<FishNetSpawner>();
            _spawner.OnSpawned += SpawnerOnOnSpawned;

            _tugboat = GetComponent<Tugboat>();
        }

        private void SpawnerOnOnSpawned(NetworkObject obj)
        {
            print($"obj: {obj.IsOwner} {obj.IsClient} {obj.IsServer} {obj.IsHost} {obj.Owner.IsLocalClient}");
        }

        private void OnApplicationQuit()
        {
            if (_tugboat != null)
            {
                try
                {
                    print("Stopping tugboat");
                    // stop client first
                    _tugboat.StopConnection(false);
                    // stop server
                    _tugboat.StopConnection(true);
                }
                catch (Exception e)
                {
                    ;
                }
            }
        }
    }
}