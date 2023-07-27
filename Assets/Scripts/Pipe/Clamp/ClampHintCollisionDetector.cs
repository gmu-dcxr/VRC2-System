using System;
using UnityEngine;

namespace VRC2.Pipe
{
    public class ClampHintCollisionDetector : MonoBehaviour
    {
        private GameObject parent;
        private ClampHintManager _hintManager;

        private void Start()
        {
            parent = gameObject.transform.parent.gameObject;
            _hintManager = parent.GetComponentInChildren<ClampHintManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                // hide self only when it's on the wall
                if (_hintManager.OnTheWall)
                {
                    gameObject.SetActive(false);
                    // remove clamp rigid body
                    var iclamp = go.transform.parent.gameObject;
                    Rigidbody rb = null;
                    if (iclamp.TryGetComponent<Rigidbody>(out rb))
                    {
                        GameObject.Destroy(rb);
                    }
                }
            }
        }
    }
}