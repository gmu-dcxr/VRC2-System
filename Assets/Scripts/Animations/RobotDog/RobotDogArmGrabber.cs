using System;
using UnityEngine;
using VRC2.Authority;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        [Header("Attachment")] public GameObject attachPoint;

        private bool grabbed;

        private void Start()
        {
            grabbed = false;
        }

        private void Update()
        {
            if (attachPoint.transform.childCount > 0)
            {
                var rot = transform.localRotation.eulerAngles;
                var left = gameObject.name.StartsWith("Left");
                if (left && rot.y < 20)
                {
                    var child = attachPoint.transform.GetChild(0).gameObject;
                    // left gripper, drop
                    child.transform.parent = null;
                    PipeHelper.AfterMove(ref child);
                    // grabbed = false;
                }
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            var go = other.gameObject;
            // var right = gameObject.name.StartsWith("Right");
            // var rot = transform.localRotation.eulerAngles;
            if (!grabbed && go.CompareTag(pipeTag))
            {
                var root = PipeHelper.GetRoot(go);
                var pm = root.GetComponent<PipeManipulation>();

                if (pm != null && root.transform.parent == null)
                {
                    root.transform.parent = attachPoint.transform;
                    root.transform.localPosition = Vector3.zero;
                    if (root.transform.parent != null)
                    {
                        grabbed = true;
                    }
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
            // var pipeTag = GlobalConstants.pipeObjectTag;
            // if (other.gameObject.CompareTag(pipeTag))
            // {
            //     if (attached != null)
            //     {
            //         attached.transform.parent = null;
            //         attached.GetComponent<Rigidbody>().useGravity = true;
            //         attached = null;
            //     }
            // }
        }
    }
}