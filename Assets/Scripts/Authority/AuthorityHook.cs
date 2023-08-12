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

        public virtual void EnableP1()
        {

        }

        public virtual void DisableP1()
        {

        }

        public virtual void EnableP2()
        {

        }

        public virtual void DisableP2()
        {

        }

        public virtual void Default()
        {
            // where this is no network
        }


        private void Update()
        {
            if (_authorityUpdated) return;

            if (_runner == null)
            {
                _runner = FindObjectOfType<NetworkRunner>();
            }

            if (_runner == null || !_runner.IsRunning)
            {
                Default();
                return;
            }

            // enable on both sides
            if (!P1Only && !P2Only)
            {
                if (_runner.IsServer)
                {
                    EnableP1();
                }
                else
                {
                    EnableP2();
                }

                _authorityUpdated = true;
                return;
            }

            // disable P2 side
            if (P1Only)
            {
                if (_runner.IsServer)
                {
                    EnableP1();
                }
                else
                {
                    DisableP2();
                }

                _authorityUpdated = true;
            }
            else if (P2Only)
            {
                if (_runner.IsServer)
                {
                    DisableP1();
                }
                else
                {
                    EnableP2();
                }

                _authorityUpdated = true;
            }
        }
    }
}