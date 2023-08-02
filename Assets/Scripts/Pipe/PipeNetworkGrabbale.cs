using Fusion;
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

        public override bool NetworkedPointerEvent(PointerEvent evt)
        {
            // process pointer event for networked pipe objects.
            // state authority and input authority are all at P1 side,
            // if game not started, directly return false to let parent handle it
            if (!gameStarted) return false;

            // if game started
            if (hasInputAuthority)
            {
                // p1 side, call the parent method
                return false;
            }
            // p2 side
            // in order to make p2 can manipulate the pipe, it should disable networkTransform component
            // when it is select action; and enable networkTransform component when it is unselect action;

            var nt = gameObject.GetComponent<NetworkTransform>();

            switch (evt.Type)
            {
                case PointerEventType.Select:
                    // let p2 to manipulate it
                    nt.enabled = false;

                    EndTransform();
                    break;
                case PointerEventType.Unselect:
                    // restore the networkTransform
                    nt.enabled = true;

                    EndTransform();
                    break;
                case PointerEventType.Cancel:
                    EndTransform();
                    break;
            }

            BaseProcessPointerEvent(evt);

            switch (evt.Type)
            {
                case PointerEventType.Select:
                    BeginTransform();
                    break;
                case PointerEventType.Unselect:
                    BeginTransform();
                    break;
                case PointerEventType.Move:
                    UpdateTransform();
                    break;
            }

            return true;
        }
    }
}