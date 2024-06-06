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
                    _wallCollisionDetector = wall.GetComponent<WallCollisionDetector>();
                }

                return _wallCollisionDetector;
            }
        }

        private float wallExtentsX => wallCollisionDetector._wallExtents.x;
        private float extentsZ => wallCollisionDetector.GetClampZBySize(clampManipulation.ClampSize);

        [HideInInspector] public bool Compensated = false;

        [HideInInspector]
        public bool collidingWall
        {
            get => clampManipulation.collidingWall;
        }

        [HideInInspector] public Pose LastCompensation = new Pose();

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
            if (provider == null || !provider.IsValid || Compensated) return;

            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            var rotation = grabPoint.rotation;

            var pos = grabPoint.position;

            // enable compensating when a single pipe collides the wall
            if (collidingWall)
            {
                print("Clamp colliding wall. Apply compensation.");
                (pos, rotation) = Compensate(pos, rotation);
                // update the cache
                LastCompensation.position = pos;
                LastCompensation.rotation = rotation;

                // do nothing once compensated
                Compensated = true;
            }

            targetTransform.rotation = rotation;
            targetTransform.position = pos;
        }

        public void EndTransform()
        {
        }

        public (Vector3, Quaternion) Compensate(Vector3 pos, Quaternion rot)
        {
            // get the wall transform
            var wt = wall.transform;
            var wpos = wt.position;

            // as the wall rotation is (0,0,0), hardcode this rotation to make the clamp always perpendicular to the wall

            var rotation = rot.eulerAngles;
            rotation.x = 0;
            rotation.y = -90;

            // update distance
            pos.x = wpos.x + wallExtentsX + 2 * extentsZ;

            rot = Quaternion.Euler(rotation);

            return (pos, rot);
        }
    }
}