using System;
using FishNet.Object;
using UnityEngine;

using FishNetSpawner = FishNet.Component.Spawning.PlayerSpawner;

namespace VRC2.Character
{
    [RequireComponent(typeof(FishNetSpawner))]
    public class FishNetSpawnerHook : MonoBehaviour
    {
        private FishNetSpawner _spawner;

        public void Start()
        {
            _spawner = GetComponent<FishNetSpawner>();
            _spawner.OnSpawned += SpawnerOnOnSpawned;
        }

        private void SpawnerOnOnSpawned(NetworkObject obj)
        {
            print($"obj: {obj.IsOwner} {obj.IsClient} {obj.IsServer} {obj.IsHost} {obj.Owner.IsLocalClient}");
            if (obj.IsOwner)
            {
                // local object
                GlobalConstants.localFishNetPlayer = obj.ObjectId;
                print($"Set localFishNetPlayer = {obj.ObjectId}");
            }
        }
    }
}