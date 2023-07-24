using System;
using Fusion;
using UnityEngine;

namespace VRC2.Authority
{
    public class AuthorityHook : MonoBehaviour
    {
        [Header("Authority")] public bool P1Only;
        public bool P2Only;

        private NetworkRunner _runner = null;
        public virtual void DisableP2()
        {

        }

        public virtual void DisableP1()
        {

        }
        

        private void Update()
        {
            if (_runner == null)
            {
                _runner = FindObjectOfType<NetworkRunner>();
            }
            
            if(!_runner.IsRunning) return;

            // enable on both sides
            if (!P1Only && !P2Only) return;

            // disable P2 side
            if (P1Only && _runner.IsClient)
            {
                DisableP2();
            }

            // disable P1 side
            if (P2Only &&  _runner.IsServer)
            {
                DisableP1();
            }
        }
    }
}