using System;
using UnityEngine;

namespace VRC2.Authority
{
    public class MeshColliderHook : AuthorityHook
    {
        private MeshCollider _collider;

        private void Start()
        {
            _collider = gameObject.GetComponent<MeshCollider>();
        }

        public override void DisableP2()
        {
            print("Disable P2 collider");
            _collider.enabled = false;
        }

        public override void DisableP1()
        {
            print("MeshColliderHook DisableP1");
        }
    }
}