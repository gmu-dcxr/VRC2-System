using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Pipe;


namespace VRC2
{
    public class PipesContainerManager : MonoBehaviour
    {
        private GameObject _controller;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void AttachToController(GameObject controller)
        {
            _controller = controller;
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
                    // add rigid body and enable interactability
                    var go = gameObject;
                    PipeHelper.AfterMove(ref go);
                }

                // synchronize transform of the parent
                var t = _controller.transform;
                transform.position = t.position;
                transform.rotation = t.rotation;
            }
        }
    }
}