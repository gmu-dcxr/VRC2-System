using Fusion;
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

        private NetworkObject _networkObject;

        private NetworkObject networkObject
        {
            get
            {
                if (_networkObject == null)
                {
                    _networkObject = gameObject.GetComponent<NetworkObject>();
                }

                return _networkObject;
            }
        }

        [HideInInspector] public bool selected = false;

        private void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        private void OnSelect()
        {
            // return if colliding on the wall for p2

            if (networkObject != null && networkObject.Runner != null && networkObject.Runner.IsClient)
            {
                if (clampManipulation.collidingWall) return;
            }

            selected = true;

            // reset 
            freeTransformer.Compensated = false;
            // make it not drop
            clampManipulation.SetKinematic(true);
            // reset
            clampManipulation.collidingWall = false;
            // reset status
            clampManipulation.UpdateStatus(false);
        }

        private void OnRelease()
        {
            selected = false;
            // compensated means colliding with the wall and being fixed --> not drop --> kinematic (true)
            clampManipulation.SetKinematic(freeTransformer.Compensated);
        }
    }
}