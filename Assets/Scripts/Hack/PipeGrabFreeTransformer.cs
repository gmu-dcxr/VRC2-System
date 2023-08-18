using System;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using VRC2.Hack;
using VRC2.Pipe;

namespace VRC2.Hack
{
    public class PipeGrabFreeTransformer : MonoBehaviour, ITransformer
    {
        private IGrabbable _grabbable;
        private Pose _grabDeltaInLocalSpace;

        private bool offsetSet = false;

        private float _zOffset = 0;

        private PipeManipulation _pipeManipulation;
        private PipesContainerManager _pipesContainerManager;

        private DistanceLimitedAutoMoveTowardsTargetProvider _provider;

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

            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;
            _grabDeltaInLocalSpace = new Pose(
                targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position),
                Quaternion.Inverse(grabPoint.rotation) * targetTransform.rotation);
        }

        public void UpdateTransform()
        {
            if (provider == null || !provider.IsValid) return;

            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            var rot = grabPoint.rotation * _grabDeltaInLocalSpace.rotation;

            // add offset
            var rotation = rot.eulerAngles;
            rotation.z += zOffset;

            var pos = grabPoint.position - targetTransform.TransformVector(_grabDeltaInLocalSpace.position);

            // enable compensating when a single pipe collides the wall
            if (collidingWall)
            {
                // print("Colliding Wall. Apply compensation.");
                (pos, rotation) = CompensateWithDirection(pos, rotation);

                foreach (var chm in clampHintManagers)
                {
                    chm.OnTheWall = true;
                }
            }
            else
            {
                foreach (var chm in clampHintManagers)
                {
                    chm.OnTheWall = false;
                }
            }

            targetTransform.rotation = Quaternion.Euler(rotation);
            targetTransform.position = pos;

        }

        public void EndTransform()
        {
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

        public (Vector3, Vector3) Compensate(Vector3 pos, Vector3 rot)
        {
            // get diameter
            var diameter = GetDiameter();

            var pipeYRotationOffset = wallCollisionDetector.pipeYRotationOffset;
            // get real diameter
            var pipez = wallCollisionDetector.GetPipeZByDiameter(diameter);

            var wallExtends = wallCollisionDetector._wallExtends;

            // get the wall transform
            var wt = wall.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // set pipe's x rotation to the wall's x rotation
            rot.x = wrot.x;
            // set pipe's y rotation to the wall's y rotation
            rot.y = wrot.y + pipeYRotationOffset;

            // update the pipe's distance to the wall
            pos.x = wpos.x + wallExtends.x + pipez;

            return (pos, rot);
        }

        public (Vector3, Vector3) CompensateWithDirection(Vector3 pos, Vector3 rot)
        {
            // print("Colliding Wall. Apply compensation.");
            var (newPos, newRot) = Compensate(pos, rot);

            var dir = (newPos - pos).normalized;
            var angle = Vector3.Angle(dir, wall.transform.right);

            // angle: 0 - controller passes through the wall, 180 - controller is outside the wall
            if (angle < 180)
            {
                rot = newRot;
                pos = newPos;
            }

            return (pos, rot);
        }



        #endregion
    }
}