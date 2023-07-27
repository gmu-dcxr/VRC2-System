using System;
using UnityEngine;

namespace VRC2.Pipe
{
    public class ClampHintCollisionDetector : MonoBehaviour
    {
        private ClampHintManager _hintManager;

        private void Start()
        {
            _hintManager = gameObject.GetComponentInParent<ClampHintManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.wallTag))
            {
                // pipe leaves the wall
                var root = PipeHelper.GetRoot(gameObject);

                // add rigid body etc.
                PipeHelper.AfterMove(ref root);

                var children = Utils.GetChildren<ClampHintManager>(root);

                foreach (var child in children)
                {
                    var chm = child.GetComponent<ClampHintManager>();
                    chm.OnTheWall = false;
                    chm.Clamped = false;
                }
            } else if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                _hintManager.Clamped = false;
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.clampObjectTag) && _hintManager.OnTheWall)
            {
                _hintManager.Clamped = true;
            }
        }
    }
}