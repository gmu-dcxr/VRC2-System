using UnityEngine;
using VRC2.Pipe;

using PipeBendCutParameters = VRC2.Pipe.PipeConstants.PipeBendCutParameters;
namespace VRC2.Events
{
    public class P1CommandAIDroneEvent : BaseEvent
    {
        private PipeBendCutParameters _parameters;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        }
        //TODO: devise AI Drone logic
        public override void Execute()
        {
            Debug.Log("AI drone is going to deliver pipes");
        }
    }
}