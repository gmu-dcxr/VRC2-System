using System;
using UnityEngine;
using VRC2.Authority;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        [Header("Attachment")] public GameObject attachPoint;

        private GameObject attached;

        private void Start()
        {
            attached = null;
        }

        private void Update()
        {
            if (attached != null)
            {
                // force update the local transformation
                attached.transform.localPosition = Vector3.zero;
                attached.transform.localRotation = Quaternion.identity;
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            var go = other.gameObject;
            if (go.CompareTag(pipeTag))
            {
                var flag = PipeHelper.IsSimpleStraightNotcutPipe(go);
                var root = PipeHelper.GetRoot(go);
                if (flag && root.transform.parent == null)
                {
                    root.GetComponent<Rigidbody>().useGravity = false;
                    root.transform.parent = attachPoint.transform;
                    attached = root;
                }
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

        private void OnTriggerExit(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            if (other.gameObject.CompareTag(pipeTag))
            {
                if (attached != null)
                {
                    attached.transform.parent = null;
                    attached.GetComponent<Rigidbody>().useGravity = true;
                    attached = null;
                }
            }
        }
    }
}