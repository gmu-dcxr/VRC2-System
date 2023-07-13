using Oculus.Interaction;
using Oculus.Interaction.DistanceReticles;
using UnityEngine;
using VRC2.Events;


namespace VRC2.Pipe
{
    public static class PipeHelper
    {
        public static void BeforeMove(ref GameObject interactablePipe)
        {
            // remove rigid body, interactable, and collision detector

            // remove its rigid body
            var rb = interactablePipe.GetComponent<Rigidbody>();
            GameObject.Destroy(rb);

            // disable its interactable ability
            var reticle = interactablePipe.GetComponentInChildren<ReticleDataIcon>();
            reticle.gameObject.SetActive(false);
        }

        public static void AfterMove(ref GameObject interactablePipe)
        {
            // restore rigid body if need
            var rb = interactablePipe.GetComponent<Rigidbody>();
            if (rb == null)
            {
                // add new one
                rb = interactablePipe.AddComponent<Rigidbody>();
            }
            // update detection method
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // enable interactable ability
            var reticle = interactablePipe.GetComponentInChildren<ReticleDataIcon>(true);
            reticle.gameObject.SetActive(true);

            // update rigid body
            var si = interactablePipe.GetComponent<SnapInteractor>();
            si.InjectRigidbody(rb);

            var dgi = interactablePipe.GetComponentInChildren<DistanceGrabInteractable>();
            dgi.InjectRigidbody(rb);
        }
    }
}