using System;
using UnityEngine;
using VRC2.Events;
using VRC2.Pipe;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeBendCutParameters = VRC2.Pipe.PipeConstants.PipeBendCutParameters;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;

namespace VRC2
{
    public class RobotBendCut: BaseEvent
    {
        [Header("Robot Setting")]
        public GameObject robot;
        
        public Transform startPoint;
        public Transform middlePoint;

        private PipeBendCutParameters _parameters;

        private GameObject _currentPipe;

        [Header("Interactable Pipe")] public GameObject spawned;


        private void Start()
        {
        }

        public void InitParameters(PipeBendAngles angle, float a, float b)
        {
            _parameters.angle = angle;
            _parameters.a = a;
            _parameters.b = b;
            // color and type and from global constants
            _currentPipe = GlobalConstants.selectedPipe;
            // get color and type
            var pm = _currentPipe.GetComponent<PipeManipulation>();
            _parameters.color = pm.pipeColor;
            _parameters.type = pm.pipeType;
            _parameters.diameter = pm.diameter;
        }

        public void PickUp()
        {
            Debug.Log("Robot PickUp");
            // get the pipe from P1, and move to the middle point where to bend/cut

            var pipe = GlobalConstants.selectedPipe;
            // set parent to robot
            pipe.transform.parent = robot.transform.parent;
            // move robot to the middle point to do bend or cut
            robot.transform.position = middlePoint.position;
        }

        public void DropOff()
        {
            Debug.Log("Robot DropOff");
            // deliver the pipe to the start point
            robot.transform.position = startPoint.position;
            // un parent
            spawned.transform.parent = null;
        }

        public void BendCut()
        {
            Debug.Log("Robot BendCut");
            // bind/cut the pipe
            
            // spawn on local side, edit the mesh and material, and update remote one
            
            // enable only current angle
            var pm = spawned.GetComponent<PipeManipulation>();
            // enable only
            pm.EnableOnly(_parameters.angle);
            // set material
            pm.SetMaterial(PipeMaterialColor.Blue);
            // edit mesh
            EditMesh(_parameters);
            // Send RPC message to remote side
            
            // sent parent to the robot
            spawned.transform.parent = robot.transform;
        }

        public void Execute()
        {
            PickUp();
            BendCut();
            DropOff();
        }

        void EditMesh(PipeBendCutParameters parameters)
        {
            var angle = parameters.angle;
            var a = parameters.a;
            var b = parameters.b;
            // TODO: 
        }
        
    }
}