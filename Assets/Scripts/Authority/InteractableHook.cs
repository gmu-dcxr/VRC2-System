using System;
using Oculus.Interaction;
using Oculus.Interaction.DistanceReticles;
using UnityEngine;

namespace VRC2.Authority
{
    public class InteractableHook: AuthorityHook
    {
        private GameObject interactable;

        private void Start()
        {
            interactable = gameObject.GetComponentInChildren<ReticleDataIcon>().gameObject;
        }

        public override void DisableP1()
        {
            interactable.SetActive(false);
        }

        public override void DisableP2()
        {
            
        }
    }
}