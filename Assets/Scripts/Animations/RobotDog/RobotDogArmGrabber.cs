using System;
using UnityEngine;
using VRC2.Authority;
using VRC2.Pipe;

namespace VRC2.Animations
{
    public class RobotDogArmGrabber : MonoBehaviour
    {
        [Header("Attachment")] public GameObject attachPoint;

        private PipeManipulation _pipeManipulation;

        private bool grabbed;

        private void Start()
        {
            _pipeManipulation = null;
            grabbed = false;
        }

        private void Update()
        {
            if (_pipeManipulation != null)
            {
                var rot = transform.localRotation.eulerAngles;
                if (rot.x == 270 && rot.y < 20)
                {
                    // left gripper, drop
                    _pipeManipulation.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    _pipeManipulation.robotArmGrabbed = false;
                    _pipeManipulation = null;
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
                // var flag = PipeHelper.IsSimpleStraightNotcutPipe(go);
                var root = PipeHelper.GetRoot(go);
                _pipeManipulation = root.GetComponent<PipeManipulation>();
                // if (flag && root.transform.parent == null)
                if (_pipeManipulation != null && !_pipeManipulation.robotArmGrabbed)
                {
                    grabbed = true;
                    root.GetComponent<Rigidbody>().useGravity = false;
                    root.transform.parent = attachPoint.transform;
                    _pipeManipulation.robotArmGrabbed = true;
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