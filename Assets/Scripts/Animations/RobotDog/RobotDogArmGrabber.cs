using System;
using UnityEngine;
using VRC2.Authority;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        [Header("Attachment")] public GameObject attachPoint;

        void OnTriggerEnterAndStay(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            var go = other.gameObject;
            if (attachPoint.transform.childCount == 0 && go.CompareTag(pipeTag))
            {
                var root = PipeHelper.GetRoot(go);
                root.transform.parent = attachPoint.transform;
                root.transform.localPosition = Vector3.zero;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }
    }
}