using Hack;
using Oculus.Interaction;
using UnityEngine;
using VRC2.Events;

namespace VRC2.Hack
{
    public class ClampNetworkGrabbable: NetworkGrabbable
    {
        protected override void Start()
        {
            this.BeginStart(ref _started, () => base.Start());

            // remove the OneGrabFreeTransformer
            var ogft = gameObject.GetComponent<OneGrabFreeTransformer>();
            GameObject.Destroy(ogft);

            // force update the OneGrabTransformer
            ClampGrabFreeTransformer defaultTransformer = gameObject.AddComponent<ClampGrabFreeTransformer>();
            _oneGrabTransformer = defaultTransformer;
            OneGrabTransformer = defaultTransformer;
            OneGrabTransformer.Initialize(this);

            this.EndStart(ref _started);
        }

        public override bool NetworkedPointerEvent(PointerEvent evt)
        {
            // use the default handler
            return false;
        }
    }
}