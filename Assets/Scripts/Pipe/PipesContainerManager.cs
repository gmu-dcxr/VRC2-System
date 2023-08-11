using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Pipe;


namespace VRC2
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipesContainerManager : MonoBehaviour
    {
        private GameObject _controller;

        private PointableUnityEventWrapper _wrapper;

        private PointableUnityEventWrapper wrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
                }

                return _wrapper;
            }
        }

        private Rigidbody _rigidbody
        {
            get => GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            wrapper.WhenSelect.AddListener(OnSelect);
            wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            if (_rigidbody == null) return;
            
            _rigidbody.isKinematic = true;
        }

        public void OnRelease()
        {
            if (_rigidbody == null) return;
            
            _rigidbody.isKinematic = false;
        }

        public void AttachToController(GameObject controller)
        {
            _controller = controller;
        }

        public bool AttachedToController()
        {
            return _controller != null;
        }

        public void DetachController()
        {
            print($"Detach object from the controller: {gameObject.name}");
            _controller = null;
        }

        // enable it can free drop
        void DisableKinematic()
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }


        // Update is called once per frame
        void Update()
        {
            // check the left controller trigger released event
            if (_controller != null)
            {
                var pressed = OVRInput.Get(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.LTouch);
                if (!pressed)
                {
                    Debug.Log("Released from the left hand controller.");
                    _controller = null;

                    DisableKinematic();
                    return;
                }

                // synchronize transform of the parent
                var t = _controller.transform;
                transform.position = t.position;
                transform.rotation = t.rotation;
            }
        }
    }
}