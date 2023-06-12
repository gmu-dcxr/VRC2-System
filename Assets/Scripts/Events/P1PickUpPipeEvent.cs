using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class P1PickUpPipeEvent : BaseEvent
    {
        public NetworkPrefabRef prefab;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Execute()
        {
            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;
            if (runner == null || localPlayer == null)
                return;
            runner.Spawn(prefab, new Vector3(0f, 1.5f, 2f), Quaternion.identity, localPlayer);
        }
    }
}
