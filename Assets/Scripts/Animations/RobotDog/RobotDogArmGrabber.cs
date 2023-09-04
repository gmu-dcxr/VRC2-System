using System;
using UnityEngine;
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

        private void OnTriggerEnter(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            var go = other.gameObject;
            if (go.CompareTag(pipeTag))
            {
                var flag = PipeHelper.IsSimpleStraightNotcutPipe(go);
                if (flag)
                {
                    var root = PipeHelper.GetRoot(go);
                    root.GetComponent<Rigidbody>().useGravity = false;
                    root.transform.parent = attachPoint.transform;
                    attached = root;
                }
            }
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