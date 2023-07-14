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
        
        public static float GetExtendsX(GameObject pipe)
        {
            var mesh = pipe.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var minx = vertices[0].x;
            var maxx = minx;

            foreach (var v in vertices)
            {
                if (v.x > maxx) maxx = v.x;
                if (v.x < minx) minx = v.x;
            }

            var p1 = Vector3.zero;
            p1.x = minx;

            var p2 = Vector3.zero;
            p2.x = maxx;

            var t = pipe.transform;

            p1 = t.TransformPoint(p1);
            p2 = t.TransformPoint(p2);

            return Vector3.Distance(p1, p2);
        }
    }
}