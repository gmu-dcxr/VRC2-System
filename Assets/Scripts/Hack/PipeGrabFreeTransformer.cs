using System;
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

        private DistanceLimitedAutoMoveTowardsTargetProvider _provider;

        private WallCollisionDetector _wallCollisionDetector;

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

        [HideInInspector]
        public bool collidingWall
        {
            get
            {
                if (_pipeManipulation == null)
                {
                    _pipeManipulation = gameObject.GetComponent<PipeManipulation>();
                }

                return _pipeManipulation.collidingWall;
            }
        }

        private GameObject _wall;

        // private GameObject wall
        // {
        //     get
        //     {
        //         if (_wall == null)
        //         {
        //             _wall = GameObject.FindGameObjectWithTag(GlobalConstants.wallTag);
        //         }
        //
        //         return wall;
        //     }
        // }

        // private WallCollisionDetector wallCollisionDetector
        // {
        //     get
        //     {
        //         if (_wallCollisionDetector == null)
        //         {
        //             _wallCollisionDetector = wall.GetComponent<WallCollisionDetector>();
        //         }
        //
        //         return _wallCollisionDetector;
        //     }
        // }


        private float zOffset
        {
            get
            {
                if (!offsetSet)
                {
                    var pm = gameObject.GetComponent<PipeManipulation>();
                    var angle = pm.angle;
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
                }

                return _zOffset;
            }
        }

        private void Start()
        {
            _wall = GameObject.FindGameObjectWithTag(GlobalConstants.wallTag);
            _wallCollisionDetector = _wall.GetComponent<WallCollisionDetector>();

            print("_wall is set");
            print("_wallCollisionDetector is set");
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

            var rotation = rot.eulerAngles;

            rotation.z += zOffset;

            // enable compensating when a single pipe collides the wall

            var pos = grabPoint.position - targetTransform.TransformVector(_grabDeltaInLocalSpace.position);

            if (collidingWall)
            {
                // print("Colliding Wall. Apply compensation.");
                (pos, rotation) = Compensate(targetTransform, pos, rotation);
            }

            targetTransform.rotation = Quaternion.Euler(rotation);
            targetTransform.position = pos;

        }

        public void EndTransform()
        {
        }

        #region Pipe Wall Compensation

        (Vector3, Vector3) Compensate(Transform target, Vector3 pos, Vector3 rot)
        {
            var pm = target.gameObject.GetComponent<PipeManipulation>();
            // get diameter
            var diameter = pm.diameter;

            var pipeYRotationOffset = _wallCollisionDetector.pipeYRotationOffset;
            // get real diameter
            var pipez = _wallCollisionDetector.GetPipeZByDiameter(diameter);

            var wallExtends = _wallCollisionDetector._wallExtends;

            // var rootObject = target.gameObject;
            // var t = rootObject.transform;
            // var pos = t.position;
            // var rot = t.rotation.eulerAngles;

            // get the wall transform
            var wt = _wall.transform;
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



        #endregion
    }
}