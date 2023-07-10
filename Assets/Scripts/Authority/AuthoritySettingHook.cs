using System;
using UnityEngine;

namespace VRC2.Authority
{
    public class AuthoritySettingHook : MonoBehaviour
    {
        [Header("Authority")] public bool P1Only;
        public bool P2Only;

        private void Start()
        {

        }

        private void Update()
        {
            // return is game not started
            if (!GlobalConstants.IsNetworkReady()) return;


            // enable on both sides
            if (!P1Only && !P2Only) return;

            // disable P2 side
            if (P1Only && GlobalConstants.Checker)
            {
                gameObject.SetActive(false);
            }

            // disable P1 side
            if (P2Only && GlobalConstants.Checkee)
            {
                gameObject.SetActive(false);
            }
        }
    }
}