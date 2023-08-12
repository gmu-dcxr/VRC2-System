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

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        public void BeginTransform()
        {
            if(provider == null || !provider.IsValid) return;
            
            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;
            _grabDeltaInLocalSpace = new Pose(
                targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position),
                Quaternion.Inverse(grabPoint.rotation) * targetTransform.rotation);
        }

        public void UpdateTransform()
        {
            if(provider == null || !provider.IsValid) return;
            
            Pose grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            var rot = grabPoint.rotation * _grabDeltaInLocalSpace.rotation;

            var rotation = rot.eulerAngles;

            rotation.z += zOffset;

            targetTransform.rotation = Quaternion.Euler(rotation);
            targetTransform.position =
                grabPoint.position - targetTransform.TransformVector(_grabDeltaInLocalSpace.position);
        }

        public void EndTransform()
        {
        }
    }
}