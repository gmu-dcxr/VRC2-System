using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    public class WaterLevelLabelUpdater : MonoBehaviour
    {
        [Header("Label")] public TextMeshPro textMeshPro;

        public NetworkPrefabRef waterLevel;

        private bool spawned = false;

        private NetworkRunner _runner
        {
            get => GlobalConstants.networkRunner;
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (spawned) return;
            UpdateInputAuthority();

            var d = GetDegree();
            textMeshPro.text = $"{d}";
        }

        int GetDegree()
        {
            var t = gameObject.transform.rotation.eulerAngles;
            return (int)(Math.Abs(t.x % 90));
        }

        void SpawnObject()
        {
            var no = gameObject.GetComponent<NetworkObject>();

            if (no != null && no.IsSceneObject)
            {
                var t = gameObject.transform;

                var runner = GlobalConstants.networkRunner;
                var player = GlobalConstants.localPlayer;
                var nid = no.NetworkGuid;
                var table = NetworkProjectConfig.Global.PrefabTable;
                //
                // NetworkPrefabId npid;
                // NetworkObject networkObject = null;
                // if (table.TryGetId(nid, out npid))
                // {
                //     table.TryGetPrefab(npid, out networkObject);
                // }
                //
                print("Spawn water level");

                runner.Spawn(waterLevel, t.position, t.rotation, player);

                // if (networkObject != null)
                // {
                //     print("Spawn water level 2");
                //     runner.Spawn(waterLevel, t.position, t.rotation, player);
                // }
            }
        }

        void UpdateInputAuthority()
        {
            var runner = GlobalConstants.networkRunner;
            var player = GlobalConstants.remotePlayer;

            // update authority on the host side
            if (runner != null && runner.IsRunning && runner.IsServer && player != PlayerRef.None)
            {
                // // SpawnObject();
                // print("assign water level");
                //
                // var no = gameObject.GetComponent<NetworkObject>();
                // no.AssignInputAuthority(player);

                // spawned = true;

                var t = gameObject.transform;

                var spo = runner.Spawn(waterLevel, t.position, t.rotation, player);

                if (spo != null)
                {
                    spawned = true;
                }
            }
        }
    }
}