using System;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipeContainerManager : MonoBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenUnselect.AddListener(OnUnselect);
            _wrapper.WhenSelect.AddListener(OnSelect);
        }

        void OnUnselect()
        {
            // enable kinematic
            _rigidbody.isKinematic = false;
        }

        void OnSelect()
        {
            _rigidbody.isKinematic = true;
        }
    }
}