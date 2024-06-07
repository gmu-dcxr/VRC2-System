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

        public void SetReference(ref GameObject c, ref GameObject o)
        {
            cip = c;
            oip = o;
        }

        #endregion

        #region Right hand control

        // use the same variables as those in PipeGrabFreeTransformer.cs

        private float _offsetFactor = 0.8f;

        private Vector3 _moveOffset = Vector3.zero;

        private float _halfPipeLength
        {
            get
            {
                var length = 0.0f;

                if (oipContact != null)
                {
                    length = PipeHelper.GetExtendsX(oipContact);
                }

                return length;
            }
        }

        // do nothing if held is a connector
        bool IsConnector
        {
            get
            {
                if (oip != null)
                {
                    name = oip.name;
                    if (oip.name.ToLower().Contains("connector")) return true;
                }

                // default return false
                return false;
            }
        }

        #endregion

        #region Compensation for connected pipes collision with the wall

        private PipeGrabFreeTransformer _freeTransformer;

        private PipeGrabFreeTransformer freeTransformer
        {
            get
            {
                if (_freeTransformer == null)
                {
                    _freeTransformer = gameObject.GetComponent<PipeGrabFreeTransformer>();
                }

                return _freeTransformer;
            }
        }

        #endregion

        private float wallExtentsX => wallCollisionDetector._wallExtents.x;

        // Start is called before the first frame update
        void Start()
        {
            wrapper.WhenSelect.AddListener(OnSelect);
            wrapper.WhenRelease.AddListener(OnRelease);
        }

        public void OnSelect()
        {
            heldByController = true;
            SetKinematic(true);
            // force move pipe away
            StartCoroutine(freeTransformer.ForceMoveAway());
        }

        public void OnRelease()
        {
            heldByController = false;

            if (freeTransformer.Compensated)
            {
                SetKinematic(!ShouldFall());
            }
            else
            {
                SetKinematic(false);
            }
        }

        public void AttachToController(GameObject controller)
        {
            _controller = controller;
            // set held by controller, it's to simulate holding
            heldByController = true;
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
            // reset offset
            _moveOffset.x = 0;
        }

        // Update is called once per frame
        void Update()
        {
            ControlConnectedPipe();

            // check the left controller trigger released event
            if (_controller != null)
            {
                // This only works when the pipe is first connected
                var pressed = OVRInput.GetUp(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.LTouch);
                if (pressed)
                {
                    Debug.Log("Released from the left hand controller.");
                    heldByController = false;
                    _controller = null;

                    // this happens when the pipes are just connected and freetransform is null
                    if (collidingWall && !ShouldFall())
                    {
                        // compensate to make it look nicer
                        SelfCompensate();

                        SetKinematic(true);
                    }
                    else
                    {
                        // make it able to fall
                        SetKinematic(false);
                    }

                    return;
                }

                var t = _controller.transform;
                var pos = t.position;
                var rot = t.rotation;
                if (collidingWall)
                {
                    // enable Compensate
                    (pos, rot) = CompensateLocal(pos, rot);
                }

                // synchronize transform of the parent
                transform.position = pos;
                transform.rotation = rot;

                if (!IsConnector && _moveOffset.x != 0)
                {
                    // make an offset
                    transform.Translate(_moveOffset, Space.Self);
                }
            }
        }

        void SetKinematic(bool enable)
        {
            if (_rigidbody == null) return;

            _rigidbody.isKinematic = enable;
        }

        public void CheckAfterUnclamp()
        {
            // this is necessary when unclamping
            if (!heldByController)
            {
                // simulate release
                freeTransformer.SimulateRelease();

                SetInteraction(ShouldFall());
            }
        }

        public void SetHeldByController(bool held)
        {
            heldByController = held;
            // release when the state is updated outside
            if (!held && freeTransformer != null)
            {
                // simulate release
                freeTransformer.SimulateRelease();
            }
        }

        public void SetInteraction(bool enable)
        {
            SetKinematic(!enable);
            SetInteractable(enable);
        }

        public (Vector3 pos, Quaternion rot) CompensateLocal(Vector3 pos, Quaternion rot)
        {
            var wt = wall.transform;
            var wpos = wt.position;

            var z = PipeHelper.GetExtendsZ(oipContact);

            // as the wall is fixed and its rotation is (0,0,0), use the hard-code rotation to save computation
            var rotation = rot.eulerAngles;
            rotation.x = 0;
            rotation.y = -90;
            // set the x
            pos.x = wpos.x + wallExtentsX + 2 * z * PipeGrabFreeTransformer.ScaleFactor;

            rot = Quaternion.Euler(rotation);

            return (pos, rot);
        }

        public void SelfCompensate()
        {
            var t = transform;
            var (pos, rot) = CompensateLocal(t.position, t.rotation);
            transform.position = pos;
            transform.rotation = rot;
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

        #region Right hand control


        private void LateUpdate()
        {
            // it should be attached to controller first
            if (_controller == null) return;

            var offset = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

            if (offset.x > 0)
            {
                _moveOffset.x = _offsetFactor * _halfPipeLength;
            }
            else if (offset.x < 0)
            {
                _moveOffset.x = -_offsetFactor * _halfPipeLength;
            }
            else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                _moveOffset.x = 0;
            }
        }

        #endregion
    }
}