using System;
using UnityEngine;
using VRC2.Authority;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        [Header("Attachment")] public GameObject attachPoint;

        private GameObject pipe;

        private bool grabbed;

        private void Start()
        {
            pipe = null;
            grabbed = false;
        }

        private void Update()
        {
            if (pipe != null)
            {
                var rot = transform.localRotation.eulerAngles;
                if (rot.x == 270 && rot.y < 20)
                {
                    // left gripper, drop
                    pipe.transform.parent = null;
                    PipeHelper.AfterMove(ref pipe);
                    grabbed = false;
                }
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var pipeTag = GlobalConstants.pipeObjectTag;
            var go = other.gameObject;
            if (!grabbed && go.CompareTag(pipeTag))
            {
                var root = PipeHelper.GetRoot(go);
                var pm = root.GetComponent<PipeManipulation>();

                if (pm != null)
                {
                    pipe = root;
                    grabbed = true;
                    PipeHelper.BeforeMove(ref pipe);
                    pipe.transform.parent = attachPoint.transform;
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