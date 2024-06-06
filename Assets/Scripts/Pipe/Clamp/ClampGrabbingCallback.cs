using Hack;
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

        private ClampGrabFreeTransformer _freeTransformer;

        private ClampGrabFreeTransformer freeTransformer
        {
            get
            {
                if (_freeTransformer == null)
                {
                    _freeTransformer = gameObject.GetComponent<ClampGrabFreeTransformer>();
                }

                return _freeTransformer;
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
            // reset 
            freeTransformer.Compensated = false;
            // make it not drop
            clampManipulation.SetKinematic(true);
            // reset
            clampManipulation.collidingWall = false;
        }

        private void OnRelease()
        {
            // compensated means colliding with the wall and being fixed --> not drop --> kinematic (true)
            clampManipulation.SetKinematic(freeTransformer.Compensated);
        }
    }
}