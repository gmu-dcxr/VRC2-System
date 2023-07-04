using System;
using NodeCanvas.Tasks.Actions;
using Unity.VisualScripting;
using UnityEngine;

namespace VRC2.Events
{
    public class PipeCollisionDetector : MonoBehaviour
    {
        [HideInInspector] public GameObject connecting;

        // to remove the seam
        private float eps = 1e-3f;
        private void Start()
        {
        }

        private void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go);
            }
        }

        void HandlePipeCollision(GameObject otherpipe)
        {
            // update connecting
            connecting = otherpipe.transform.parent.gameObject; // Interactable pipe

            // current interactable pipe
            var cip = gameObject.transform.parent;

            if (connecting.transform.rotation != cip.rotation)
            {
                // update rotation first, and update position in the next loop
                connecting.transform.rotation = cip.rotation;
            }
            else
            {
                // update position
                var cbounds = gameObject.GetComponent<Renderer>().bounds;
                var obounds = gameObject.GetComponent<Renderer>().bounds;

                // expected distance
                var ed = cbounds.extents.x + obounds.extents.x - eps;

                connecting.transform.position = cip.position + connecting.transform.right * ed;
            }
        }
    }
}