using Oculus.Interaction;
using UnityEngine;

using VRC2.Events;

namespace VRC2.Pipe
{
    public class PipeNetworkGrabbale : NetworkGrabbable
    {
        protected override void Start()
        {
            this.BeginStart(ref _started, () => base.Start());

            // remove the OneGrabFreeTransformer
            var ogft = gameObject.GetComponent<OneGrabFreeTransformer>();
            GameObject.Destroy(ogft);
            
            // force update the OneGrabTransformer
            PipeGrabFreeTransformer defaultTransformer = gameObject.AddComponent<PipeGrabFreeTransformer>();
            _oneGrabTransformer = defaultTransformer;
            OneGrabTransformer = defaultTransformer;
            OneGrabTransformer.Initialize(this);

            this.EndStart(ref _started);
        }
    }
}