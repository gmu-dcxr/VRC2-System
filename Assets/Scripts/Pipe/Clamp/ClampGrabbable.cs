using System.Collections.Generic;
using Fusion;
using UnityEngine;

using Oculus.Interaction;
using Unity.VisualScripting;

namespace VRC2.Events
{
    public class ClampGrabbable: GeneralGrabbable
    {
        [Header("Clamps")] [SerializeField] private NetworkPrefabRef _clampSize1;
        [SerializeField] private NetworkPrefabRef _clampSize2;
        [SerializeField] private NetworkPrefabRef _clampSize3;
        [SerializeField] private NetworkPrefabRef _clampSize4;
        
        private ClampScaleInitializer _clampScaleInitializer;
        public override void SpawnNetworkObject()
        {
            // 
            Debug.Log("Require spawning network object");

            if (_clampScaleInitializer == null)
            {
                _clampScaleInitializer = gameObject.GetComponentInChildren<ClampScaleInitializer>();
            }
            
            // get size
            var size = _clampScaleInitializer.clampSize;

            NetworkPrefabRef prefab = NetworkPrefabRef.Empty;
            switch (size)
            {
                case 1:
                    prefab = _clampSize1;
                    break;
                case 2:
                    prefab = _clampSize2;
                    break;
                case 3:
                    prefab = _clampSize3;
                    break;
                case 4:
                    prefab = _clampSize4;
                    break;
            }

            if (prefab != null && prefab != NetworkPrefabRef.Empty)
            {
                // spawn object
                var runner = GlobalConstants.networkRunner;
                var localPlayer = GlobalConstants.localPlayer;

                var t = gameObject.transform;
                var pos = t.position;

                // make it a bit closer to the camera
                var offset = Camera.main.transform.up;
                pos += offset * 0.1f;

                var no = runner.Spawn(prefab, pos, t.rotation, localPlayer);
                base.UpdateCapacityAfterSpawn(no);
            }
            else
            {
                Debug.LogError("Clamp prefab is not valid.");
            }

        }
    }
}