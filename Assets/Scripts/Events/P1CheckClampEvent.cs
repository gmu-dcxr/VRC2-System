using UnityEngine;

namespace VRC2.Events
{
    public class P1CheckClampEvent: BaseEvent
    {
        public override void Execute()
        {
            Debug.Log("Ask the experimenter to refill the clamp");
        }
    }
}