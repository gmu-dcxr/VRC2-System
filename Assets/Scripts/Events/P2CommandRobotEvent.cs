using NodeCanvas.Tasks.Actions;
using UnityEngine;
using VRC2.Pipe;

namespace VRC2.Events
{
    public class P2CommandRobotEvent : BaseEvent
    {
        public BendCutMenuController _bendCutMenuController;

        public void Start()
        {
            _bendCutMenuController.OnConfirmed += OnConfirmed;
            
            // hide at the beginning
            _bendCutMenuController.Hide();
        }

        private void OnConfirmed()
        {
            var parameter = _bendCutMenuController.result;
            // TODO: let robot bend or cut the pipe
            Debug.Log("Robot is going to bend or cut.");
            Debug.Log(parameter.ToString());
        }

        // TODO: What are required to do bend or cut?
        public override void Execute()
        {
            _bendCutMenuController.Show();
        }
    }
}