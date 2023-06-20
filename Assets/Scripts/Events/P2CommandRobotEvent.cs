using UnityEngine;

namespace VRC2.Events
{
    public class P2CommandRobotEvent : BaseEvent
    {
        // TODO: What are required to do bend or cut?
        public override void Execute()
        {
            // update event
            dialogManager.modalDialog.currentEvent = PipeInstallEvent.P2CommandRobotBendOrCut;
            
            // TODO: let robot bend or cut the pipe
            Debug.Log("Robot is going to bend or cut.");
        }

    }
}