using System;
using Oculus.Interaction;
using Oculus.Interaction.DistanceReticles;
using UnityEngine;
using VRC2.Pipe;
using VRC2.Scenarios;

namespace VRC2.Authority
{
    public class InteractableLevelHook : AuthorityHook
    {
        public override void DisableP1()
        {
            PipeHelper.DisableInteraction(gameObject);
        }

        public override void DisableP2()
        {

        }
    }
}