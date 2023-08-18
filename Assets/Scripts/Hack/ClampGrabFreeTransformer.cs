using Oculus.Interaction;
using UnityEngine;
using VRC2;
using VRC2.Hack;
using VRC2.Pipe.Clamp;

namespace Hack
{
    public class ClampGrabFreeTransformer : MonoBehaviour, ITransformer
    {
        private IGrabbable _grabbable;
        private Pose _grabDeltaInLocalSpace;

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

        private ClampManipulation _clampManipulation;

        private ClampManipulation clampManipulation
        {
            get
            {
                if (_clampManipulation == null)
                {
                    _clampManipulation = gameObject.GetComponent<ClampManipulation>();
                }

                return _clampManipulation;
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

        [HideInInspector]
        public bool collidingWall
        {
            get => clampManipulation.collidingWall;
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
            if (provider == null || !provider.IsValid || clampManipulation.compensated) return;

            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            var rot = grabPoint.rotation * _grabDeltaInLocalSpace.rotation;

            var rotation = rot.eulerAngles;

            var pos = grabPoint.position - targetTransform.TransformVector(_grabDeltaInLocalSpace.position);

            // enable compensating when a single pipe collides the wall
            if (collidingWall)
            {
                // print("Colliding Wall. Apply compensation.");
                (pos, rotation) = CompensateWithDirection(pos, rotation);

                // make it not fall
                clampManipulation.SetKinematic(true);
                // update flag
                clampManipulation.compensated = true;
            }

            targetTransform.rotation = Quaternion.Euler(rotation);
            targetTransform.position = pos;

        }

        public void EndTransform()
        {
        }

        public (Vector3, Vector3) Compensate(Vector3 pos, Vector3 rot)
        {
            // get clamp z
            var clampz = clampManipulation.GetClampExtendsZ();

            // get the wall transform
            var wt = wall.transform;
            var wpos = wt.position;
            var wrot = wt.rotation.eulerAngles;

            // clamp has the same x rotation with the wall
            rot.x = wrot.x;
            rot.y = wrot.y + wallCollisionDetector.clampYRotationOffset;

            // update distance
            pos.x = wpos.x + wallCollisionDetector._wallExtends.x + clampz * 2;

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
    }
}