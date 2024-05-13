using System.Collections;
using System.Collections.Generic;
using Fusion;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Hack;
using VRC2.Pipe;
using VRC2.ScenariosV2.Tool;


namespace VRC2
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipesContainerManager : NetworkBehaviour
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
                        if (chm.CanShow)
                        {
                            // only add valid chm
                            _clampHintsManagers.Add(chm);
                        }
                    }
                }

                return _clampHintsManagers;
            }
        }

        #endregion


        #region Distance Grab Interactable

        // enable/disable to let the pipe interactable/not-interactable

        public DistanceGrabInteractable distanceGrabInteractable;

        private void SetInteractable(bool enabled)
        {
            if (distanceGrabInteractable.enabled == enabled) return;

            distanceGrabInteractable.enabled = enabled;
        }

        #endregion

        #region Wall

        private GameObject _wall;

        private GameObject wall
        {
            get
            {
                if (_wall == null)
                {
                    print("_wall is set");
                    _wall = GameObject.FindGameObjectWithTag(GlobalConstants.wallTag);
                }

                return _wall;
            }
        }

        private WallCollisionDetector _wallCollisionDetector;

        private WallCollisionDetector wallCollisionDetector
        {
            get
            {
                if (_wallCollisionDetector == null)
                {
                    print("_wallCollisionDetector is set");
                    _wallCollisionDetector = wall.GetComponent<WallCollisionDetector>();
                }

                return _wallCollisionDetector;
            }
        }

        #endregion

        #region References

        [Header("Children")] [ReadOnly] public GameObject cip;

        [ReadOnly] public GameObject oip;
        [ReadOnly] public GameObject oipContact;

        // To control the pipe after being connected
        public void SetReference(ref GameObject c, ref GameObject o, ref GameObject contact)
        {
            cip = c;
            oip = o;
            oipContact = contact;
        }

        #endregion

        private PipeGrabFreeTransformer transformer;

        [HideInInspector] public bool selfCompensated = false;

        // Start is called before the first frame update
        void Start()
        {
            wrapper.WhenSelect.AddListener(OnSelect);
            wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            heldByController = true;
            selfCompensated = false;

            if (_rigidbody == null) return;

            _rigidbody.isKinematic = true;
        }

        public void OnRelease()
        {
            heldByController = false;

            // compensate to make it look nicer
            SelfCompensate();

            if (_rigidbody == null) return;

            _rigidbody.isKinematic = false;
        }

        public void AttachToController(GameObject controller)
        {
            _controller = controller;
            // set held by controller, it's to simulate holding
            heldByController = true;

            selfCompensated = false;
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
            heldByController = false;
        }

        // Update is called once per frame
        void Update()
        {
            ControlConnectedPipe();

            // check the left controller trigger released event
            if (_controller != null)
            {
                // This only works when the pipe is first connected
                var pressed = OVRInput.Get(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.LTouch);
                if (!pressed)
                {
                    Debug.Log("Released from the left hand controller.");
                    heldByController = false;
                    _controller = null;

                    if (collidingWall && !ShouldFall())
                    {
                        // compensate to make it look nicer
                        SelfCompensate();

                        _rigidbody.isKinematic = true;
                    }
                    else
                    {
                        // make it able to fall
                        _rigidbody.isKinematic = false;
                    }

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
                }

                // synchronize transform of the parent
                transform.position = pos;
                transform.rotation = Quaternion.Euler(rot);
            }
            else
            {
                // _controller is none, it is selected by controller
                if (!heldByController && _rigidbody != null)
                {
                    if (ShouldFall())
                    {
                        _rigidbody.isKinematic = false;
                        // make it interactable
                        SetInteractable(true);
                    }
                    else
                    {
                        _rigidbody.isKinematic = true;
                        // make it not interactable
                        SetInteractable(false);
                    }
                }
            }
        }

        public void SelfCompensate()
        {
            if (selfCompensated) return;

            if (transformer == null)
            {
                transformer = gameObject.GetComponent<PipeGrabFreeTransformer>();
            }

            var t = gameObject.transform;
            var (pos, rot) = transformer.CompensateWithDirection(t.position, t.rotation.eulerAngles);

            gameObject.transform.position = pos;
            gameObject.transform.rotation = Quaternion.Euler(rot);

            selfCompensated = true;
        }

        #region Check if pipe can drop when clamphint changes

        bool ShouldFall()
        {
            foreach (var chm in clampHintsManagers)
            {
                if (chm.Clamped) return false;
            }

            return true;
        }

        #endregion

        #region Connected pipe control

        void ControlConnectedPipe()
        {
            if (!heldByController) return;

            var pressed = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.LTouch);
            if (pressed)
            {
                var cid = cip.GetComponent<NetworkObject>().Id;

                // calculate the left rotation after rotating
                // get relative transform under the other pipe contact part
                var ot = oipContact.transform;
                var p = ot.InverseTransformPoint(cip.transform.position);
                var rf = ot.InverseTransformVector(cip.transform.forward);
                var ru = ot.InverseTransformVector(cip.transform.up);
                // rotate ot
                oipContact.transform.Rotate(Vector3.right, 90, Space.Self);
                // change it backup to the word coordinate
                ot = oipContact.transform;
                p = ot.TransformPoint(p);
                rf = ot.TransformVector(rf);
                ru = ot.TransformVector(ru);
                // update cip
                cip.transform.position = p;
                cip.transform.rotation = Quaternion.LookRotation(rf, ru);
                // sync it
                var localPos = cip.transform.localPosition;
                var localRot = cip.transform.localRotation;

                SyncCIPRotation(cid, localPos, localRot);
            }
        }

        void SyncCIPRotation(NetworkId cid, Vector3 localPos, Quaternion localRot)
        {
            if (Runner != null && Runner.IsRunning)
            {
                RPC_SendMessage(cid, localPos, localRot);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(NetworkId cid, Vector3 localPos, Quaternion localRot, RpcInfo info = default)
        {
            // sync local rotation of other pipe (the last connected pipe/connector)
            if (info.IsInvokeLocal)
            {
                print($"SyncCIPRotation {cid}");
            }
            else
            {
                print($"SyncCIPRotation of {cid}");
                var go = Runner.FindObject(cid).gameObject;
                go.transform.localPosition = localPos;
                go.transform.localRotation = localRot;
            }
        }

        #endregion
    }
}