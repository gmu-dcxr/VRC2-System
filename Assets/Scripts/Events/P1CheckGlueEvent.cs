using UnityEngine;

namespace VRC2.Events
{
    public class P1CheckGlueEvent: BaseEvent
    {
        public override void Execute()
        {
            Debug.Log("Ask the experimenter to refill the glue");
        }
    }
}