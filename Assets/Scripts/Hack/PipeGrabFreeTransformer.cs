using System;
using System.Collections;
using System.Collections.Generic;
using GameKit.Utilities;
using UnityEngine;
using Oculus.Interaction;
using VRC2.Hack;
using VRC2.Pipe;

namespace VRC2.Hack
{
    public class PipeGrabFreeTransformer : MonoBehaviour, ITransformer
    {
        private IGrabbable _grabbable;

        private bool offsetSet = false;

        private float _zOffset = 0;

        private PipeManipulation _pipeManipulation;
        private PipesContainerManager _pipesContainerManager;

        private DistanceLimitedAutoMoveTowardsTargetProvider _provider;

        // move pipe on hand for the convenience of clamping/connecting
        private float _offsetFactor = 0.8f;

        private Vector3 _moveOffset = Vector3.zero;

        private float _halfPipeLength
        {
            get
            {
                var length = 0.0f;
                if (pipeManipulation != null)
                {
                    // simple pipe
                    length = pipeManipulation.GetSegmentALength();
                }
                else
                {
                    if (pipesContainerManager != null)
                    {
                        // connected pipe
                        var oipContact = pipesContainerManager.oipContact;
                        length = PipeHelper.GetExtendsX(oipContact);
                    }
                }

                return length;
            }
        }

        private float _extentsZ
        {
            get
            {
                var z = 0.0f;
                if (pipeManipulation != null)
                {
                    // simple pipe
                    z = pipeManipulation.GetRealDiameter();
                }
                else
                {
                    if (pipesContainerManager != null)
                    {
                        // connected pipe
                        var oipContact = pipesContainerManager.oipContact;
                        z = PipeHelper.GetExtendsZ(oipContact);
                    }
                }

                return z;
            }
        }

        [HideInInspector]
        public DistanceLimitedAutoMoveTowardsTargetProvider provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = gameObject.GetComponentInChildren<DistanceLimitedAutoMoveTowardsTargetProvider>();
                }

                return _provider;
            }
        }

        #region Gameobject Type Check

        private bool pipeTypeChecked = false;

        private bool _isSimplePipe;

        [HideInInspector]
        public bool isSimplePipe
        {
            get
            {
                if (!pipeTypeChecked)
                {
                    // simple pipe owns PipeManipulation component, while pipe container doesn't
                    _isSimplePipe = gameObject.GetComponent<PipeManipulation>() != null;
                    pipeTypeChecked = true;
                }

                return _isSimplePipe;
            }
        }

        #endregion

        private PipeManipulation pipeManipulation
        {
            get
            {
                if (_pipeManipulation == null)
                {
                    _pipeManipulation = gameObject.GetComponent<PipeManipulation>();
                }

                return _pipeManipulation;
            }
        }

        private PipesContainerManager pipesContainerManager
        {
            get
            {
                if (_pipesContainerManager == null)
                {
                    _pipesContainerManager = gameObject.GetComponent<PipesContainerManager>();
                }

                return _pipesContainerManager;
            }
        }

        [HideInInspector]
        public bool collidingWall
        {
            get
            {
                if (isSimplePipe)
                {
                    return pipeManipulation.collidingWall;
                }
                else
                {
                    return pipesContainerManager.collidingWall;
                }
            }
        }

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

        private float wallExtentsX => wallCollisionDetector._wallExtents.x;

        // after compensated is true and before trigger is released, only z rotation and y and z translation are supported
        [HideInInspector] public bool Compensated = false;

        private bool forceMoving = false;

        #region Clamp Hints Managers

        private List<ClampHintManager> _clampHintManagers;

        private List<ClampHintManager> clampHintManagers
        {
            get
            {
                if (isSimplePipe)
                {
                    return pipeManipulation.clampHintsManagers;
                }
                else
                {
                    return pipesContainerManager.clampHintsManagers;
                }
            }
        }



        #endregion


        private float zOffset
        {
            get
            {
                if (isSimplePipe && !offsetSet)
                {
                    // this will happen in connected pipe mode
                    var angle = pipeManipulation.angle;

                    _zOffset = 0;
                    switch (angle)
                    {
                        case PipeConstants.PipeBendAngles.Angle_0:
                            break;
                        case PipeConstants.PipeBendAngles.Angle_45:
                            _zOffset = -180;
                            break;
                        case PipeConstants.PipeBendAngles.Angle_90:
                            _zOffset = -135;
                            break;
                        case PipeConstants.PipeBendAngles.Angle_135:
                            _zOffset = -180;
                            break;
                        default:
                            break;
                    }

                    offsetSet = true;
                }

                return _zOffset;
            }
        }

        private void Start()
        {
        }

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        public void BeginTransform()
        {
            if (provider == null || !provider.IsValid) return;
        }

        public void UpdateTransform()
        {
            if (provider == null || !provider.IsValid || forceMoving) return;

            Pose grabPoint = _grabbable.GrabPoints[0];
            var rot = grabPoint.rotation;
            var pos = grabPoint.position;

            var targetTransform = _grabbable.Transform;

            // add offset
            var rotation = rot.eulerAngles;
            rotation.z += zOffset;
            rot = Quaternion.Euler(rotation);

            // enable compensating
            if (collidingWall)
            {
                // print("Colliding Wall. Apply compensation.");
                (pos, rot) = Compensate(targetTransform, pos, rot);
                // update flag
                Compensated = true;
            }

            targetTransform.rotation = rot;
            targetTransform.position = pos;
            // translate offset
            targetTransform.Translate(_moveOffset, Space.Self);
        }

        public void EndTransform()
        {
            // unset
            _moveOffset.x = 0;
        }

        #region Pipe Wall Compensation

        public PipeConstants.PipeDiameter GetDiameter()
        {
            if (isSimplePipe)
            {
                return pipeManipulation.diameter;
            }
            else
            {
                return pipesContainerManager.diameter;
            }
        }

        public (Vector3, Quaternion) Compensate(Transform target, Vector3 pos, Quaternion rot)
        {
            // get the wall transform
            var wt = wall.transform;
            var wpos = wt.position;

            // as the wall is fixed and its rotation is (0,0,0), use the hard-code rotation to save computation
            var rotation = rot.eulerAngles;
            rotation.x = 0;
            rotation.y = -90;

            // set the x
            pos.x = wpos.x + wallExtentsX + 2 * _extentsZ;

            rot = Quaternion.Euler(rotation);

            return (pos, rot);
        }

        public IEnumerator ForceMovePipeAway()
        {
            if (Compensated)
            {
                forceMoving = true;
                var pos = _grabbable.Transform.position;
                pos.x += 2 * _extentsZ;

                _grabbable.Transform.position = pos;

                yield return new WaitForSeconds(0.5f);

                Compensated = false;
                forceMoving = false;
            }

            yield return null;
        }

        public void SimulateRelease()
        {
            print("SimulateRelease");
            var png = GetComponent<PipeNetworkGrabbable>();
            var evt = png.lastPointerEvent;
            // change type
            var evt2 = new PointerEvent(evt.Identifier, PointerEventType.Unselect, evt.Pose, evt.Data);
            png.OriginalProcessPointerEvent(evt2);
        }

        public (Vector3, Quaternion) Compensate(Vector3 pos, Quaternion rot)
        {
            var wt = wall.transform;
            var wpos = wt.position;

            // as the wall is fixed and its rotation is (0,0,0), use the hard-code rotation to save computation
            var rotation = rot.eulerAngles;
            rotation.x = 0;
            rotation.y = -90;
            // set the x
            pos.x = wpos.x + wallExtentsX + 2 * _extentsZ;

            rot = Quaternion.Euler(rotation);

            return (pos, rot);
        }


        #endregion

        bool IsConnector
        {
            get
            {
                // it should be root object
                if (gameObject.transform.parent != null) return true;

                // simple case
                var name = gameObject.name;
                if (name.ToLower().Contains("connector")) return true;
                // container case
                if (pipesContainerManager != null)
                {
                    var oip = pipesContainerManager.oip;
                    name = oip.name;
                    if (name.ToLower().Contains("connector")) return true;
                }

                // default return false
                return false;
            }
        }

        private void LateUpdate()
        {
            if (provider == null || !provider.IsValid || IsConnector) return;

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
    }
}