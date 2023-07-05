using System;
using NodeCanvas.Tasks.Actions;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRC2.Events
{
    public class PipeCollisionDetector : MonoBehaviour
    {
        [HideInInspector] public GameObject connecting;
        private Object pipeParent;
        
        private bool connected = false;

        // to remove the seam
        private float eps = 1e-3f;
        private void Start()
        {
            // pre-load object
            pipeParent = AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
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
            else if (!connected)
            {
                // update position
                var cbounds = gameObject.GetComponent<Renderer>().bounds;
                var obounds = gameObject.GetComponent<Renderer>().bounds;

                // expected distance
                var ed = cbounds.extents.x + obounds.extents.x - eps;

                connecting.transform.position = cip.position + connecting.transform.right * ed;

                var pos = cip.position + connecting.transform.right * ed / 2.0f;
                var rot = cip.rotation;

                var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;

                // update parent to make them move together
                cip.transform.parent = parentObject.transform;
                connecting.transform.parent = parentObject.transform;

                // disable children's interactions
                DisableInteraction(cip.gameObject);
                DisableInteraction(connecting.gameObject);

                connected = true;
            }
        }

        void DisableInteraction(GameObject interactable)
        {
            // disable GrabInteractable who own ReticleDataIcon
            var go = interactable.GetComponentInChildren<ReticleDataIcon>();
            if (go != null)
            {
                go.gameObject.SetActive(false);
            }
        }
    }
}