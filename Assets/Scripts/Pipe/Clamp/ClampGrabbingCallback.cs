using System;
using Oculus.Interaction;
using UnityEngine;

namespace VRC2.Events
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class ClampGrabbingCallback : MonoBehaviour
    {
        private PointableUnityEventWrapper _wrapper;

        private void Start()
        {
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenUnselect.AddListener(OnUnselect);
        }

        private void OnSelect()
        {
            Rigidbody rb = null;

            if (gameObject.TryGetComponent<Rigidbody>(out rb))
            {
                rb.useGravity = false;
            }
        }

        private void OnUnselect()
        {
            Rigidbody rb = null;

            if (gameObject.TryGetComponent<Rigidbody>(out rb))
            {
                rb.useGravity = true;
            }
        }
    }
}