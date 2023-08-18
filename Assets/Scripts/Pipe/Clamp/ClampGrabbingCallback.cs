using System;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Pipe.Clamp;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class ClampGrabbingCallback : MonoBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private ClampManipulation _clampManipulation;

        private ClampManipulation clampManipulation
        {
            get
            {
                if (_clampManipulation == null)
                {
                    _clampManipulation = gameObject.GetComponent<ClampManipulation>();
                }

                return _clampManipulation;
            }
        }

        private void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        private void OnSelect()
        {
            clampManipulation.SetKinematic(false);
            // reset
            clampManipulation.collidingWall = false;
            clampManipulation.compensated = false;
        }

        private void OnRelease()
        {
            
        }
    }
}