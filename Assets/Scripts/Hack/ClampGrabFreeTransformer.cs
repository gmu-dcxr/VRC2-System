using System.Collections;
using Oculus.Interaction;
using UnityEngine;
using VRC2;
using VRC2.Hack;
using VRC2.Pipe.Clamp;
using VRC2.Utility;

namespace Hack
{
    public class ClampGrabFreeTransformer : MonoBehaviour, ITransformer
    {
        private IGrabbable _grabbable;

        private DistanceLimitedAutoMoveTowardsTargetProvider _provider;

        public bool enableCompensation = false;

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

        #region Haptic feedback

        private VRHelper _vrHelper;

        private VRHelper vrHelper
        {
            get
            {
                if (_vrHelper == null)
                {
                    _vrHelper = FindObjectOfType<VRHelper>();
                    if (_vrHelper == null)
                    {
                        Debug.LogError("Failed to find VRHelper.");
                    }
                }

                return _vrHelper;
            }
        }

        OVRInput.Controller GetController()
        {
            var result = OVRInput.Controller.RTouch;

            var left = Vector3.Distance(transform.position, vrHelper.leftVisual.transform.position);
            var right = Vector3.Distance(transform.position, vrHelper.rightVisual.transform.position);

            if (left < right)
            {
                result = OVRInput.Controller.LTouch;
            }

            return result;
        }

        void VibrateFeedback()
        {
            StartCoroutine(Vibrate(1f));
        }

        IEnumerator Vibrate(float duration)
        {
            var ctl = GetController();
            var amp = 1.0f;
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Hand, 0f, amp, ctl);
            }

            yield return null;
        }


        #endregion


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
                // add feedback
                VibrateFeedback();
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
            if (!enableCompensation)
            {
                return (pos, rot);
            }
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