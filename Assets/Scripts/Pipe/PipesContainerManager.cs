using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Hack;
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
            get => gameObject.GetComponent<Rigidbody>();
        }

        [HideInInspector] public bool collidingWall { get; set; }

        // store the diameter of the children objects
        [HideInInspector] public PipeConstants.PipeDiameter diameter;

        [HideInInspector] public bool heldByController = false;

        #region Clamp Hints

        private List<ClampHintManager> _clampHintsManagers;

        [HideInInspector]
        public List<ClampHintManager> clampHintsManagers
        {
            get
            {
                if (_clampHintsManagers == null)
                {
                    _clampHintsManagers = new List<ClampHintManager>();

                    var children = Utils.GetChildren<ClampHintManager>(gameObject);

                    foreach (var child in children)
                    {
                        var chm = child.GetComponent<ClampHintManager>();
                        _clampHintsManagers.Add(chm);
                    }
                }

                return _clampHintsManagers;
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            wrapper.WhenSelect.AddListener(OnSelect);
            wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            heldByController = true;

            if (_rigidbody == null) return;

            _rigidbody.isKinematic = true;
        }

        public void OnRelease()
        {
            heldByController = false;

            if (_rigidbody == null) return;

            _rigidbody.isKinematic = false;
        }

        public void AttachToController(GameObject controller)
        {
            _controller = controller;
        }

        public void UpdateDiameter(PipeConstants.PipeDiameter d)
        {
            diameter = d;
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

        // Update is called once per frame
        void Update()
        {
            // check the left controller trigger released event
            if (_controller != null)
            {
                // This only works when the pipe is first connected
                var pressed = OVRInput.Get(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.LTouch);
                if (!pressed)
                {
                    Debug.Log("Released from the left hand controller.");
                    _controller = null;

                    // make it able to fall
                    _rigidbody.isKinematic = false;
                    return;
                }

                var t = _controller.transform;
                var pos = t.position;
                var rot = t.rotation.eulerAngles;
                if (collidingWall)
                {
                    // enable Compensate
                    var pgft = gameObject.GetComponent<PipeGrabFreeTransformer>();
                    (pos, rot) = pgft.CompensateWithDirection(pos, rot);
                    
                    // update CHM flag
                    foreach (var chm in clampHintsManagers)
                    {
                        chm.OnTheWall = true;
                    }
                }
                else
                {
                    foreach (var chm in clampHintsManagers)
                    {
                        chm.OnTheWall = false;
                    }
                }

                // synchronize transform of the parent
                transform.position = pos;
                transform.rotation = Quaternion.Euler(rot);
            }
        }
    }
}