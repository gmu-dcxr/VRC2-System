using Oculus.Interaction.DistanceReticles;
using UnityEngine;

namespace VRC2.Authority
{
    public class GrabbableHook : AuthorityHook
    {
        private GameObject grabInteractable
        {
            get
            {
                var rdi = gameObject.GetComponentInChildren<ReticleDataIcon>();
                if (rdi != null) return rdi.gameObject;
                return null;
            }
        }

        public override void DisableP1()
        {
            grabInteractable?.SetActive(false);
        }

        public override void DisableP2()
        {
            grabInteractable?.SetActive(false);
        }

        public override void Default()
        {
            grabInteractable?.SetActive(true);
        }
    }
}