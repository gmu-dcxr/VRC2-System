using System;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class ClampGrabbingCallback : MonoBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private Rigidbody _rigidbody;

        private Rigidbody rigidbody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = gameObject.GetComponent<Rigidbody>();
                }

                return _rigidbody;
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
            // enable
            rigidbody.isKinematic = true;
        }

        private void OnRelease()
        {
            // disable
            rigidbody.isKinematic = false;
        }
    }
}