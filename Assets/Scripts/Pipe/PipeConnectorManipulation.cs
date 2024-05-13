using System;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2.Pipe
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipeConnectorManipulation : MonoBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private Rigidbody _rigidbody
        {
            get => GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            // enable kinematic
            _rigidbody.isKinematic = true;
        }

        public void OnRelease()
        {
            // disable kinematic
            _rigidbody.isKinematic = false;
        }
    }
}