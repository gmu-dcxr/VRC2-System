using System;
using Fusion;
using UnityEngine;

namespace VRC2.Authority
{
    public class AuthorityHook : MonoBehaviour
    {
        [Header("Authority")] public bool P1Only;
        public bool P2Only;

        private bool _authorityUpdated = false;

        private NetworkRunner _runner = null;
        public virtual void DisableP2()
        {

        }

        public virtual void DisableP1()
        {

        }

        public virtual void Default()
        {
            // where this is no network
        }
        

        private void Update()
        {
            if(_authorityUpdated) return;
            
            if (_runner == null)
            {
                _runner = FindObjectOfType<NetworkRunner>();
            }

            if (_runner == null || !_runner.IsRunning)
            {
                _authorityUpdated = true;
                Default();
                return;
            }

            // enable on both sides
            if (!P1Only && !P2Only) return;

            // disable P2 side
            if (P1Only && _runner.IsClient)
            {
                _authorityUpdated = true;
                DisableP2();
            }

            // disable P1 side
            if (P2Only &&  _runner.IsServer)
            {
                _authorityUpdated = true;
                DisableP1();
            }
        }
    }
}