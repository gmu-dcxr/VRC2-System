using UnityEngine;

namespace VRC2.Events
{
    public class P1CommandAIDroneEvent : BaseEvent
    {
        //TODO: devise AI Drone logic
        public override void Execute()
        {
            Debug.Log("AI drone is going to deliver pipes");
        }
    }
}