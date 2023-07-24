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
            
            print("update");
            // return is game not started
            // if (!GlobalConstants.IsNetworkReady()) return;

            print($"{P1Only} - {P2Only}");

            // enable on both sides
            if (!P1Only && !P2Only) return;

            // disable P2 side
            if (P1Only && _runner.IsClient)
            {
                print("AuthorityHook P1Only");
                DisableP2();
                // gameObject.SetActive(false);
            }

            // disable P1 side
            if (P2Only &&  _runner.IsServer)
            {
                print("AuthorityHook P2Only");
                DisableP1();
                // gameObject.SetActive(false);
            }
        }
    }
}