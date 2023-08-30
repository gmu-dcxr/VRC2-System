using UnityEngine.InputSystem;
using UnityEngine;


namespace VRC2.Animations
{
    public class RobotArmInputRecording : BaseInputRecording
    {

        public RoboticArm move;
        public RobotArmInputAction armIA;
        public float rotateSpeed = 20f;

        private InputAction part0;
        private InputAction part1;
        private InputAction part2;
        private InputAction part3;


        public override void InitInputActions()
        {
            armIA = new RobotArmInputAction();
            armIA.Enable();

            part0 = armIA.RobotArm.part0;
            part1 = armIA.RobotArm.part1;
            part2 = armIA.RobotArm.part2;
            part3 = armIA.RobotArm.part3;
        }

        public override void DisposeInputActions()
        {
            armIA.Dispose();
        }

        public override void UpdateLogic() 
        {
            //part0 up
            if (part0.ReadValue<Vector2>().y > 0) 
            {
                move.rotatePart0(rotateSpeed);
            }
            //part0 down
            if (part0.ReadValue<Vector2>().y < 0) 
            {
                move.rotatePart0(rotateSpeed * -1f);
            }
            //part1 up
            if (part1.ReadValue<Vector2>().y > 0) 
            {
                move.rotatePart1(rotateSpeed);
            }
            //part1 down
            if (part1.ReadValue<Vector2>().y < 0)
            {
                move.rotatePart1(rotateSpeed * -1f);
            }
            //part2 up
            if (part2.ReadValue<Vector2>().y > 0) 
            {
                move.rotatePart2(rotateSpeed);
            }
            //part2 down
            if (part2.ReadValue<Vector2>().y < 0) 
            {
                move.rotatePart2(rotateSpeed * -1f);
            }
            //part3 up
            if (part3.ReadValue<Vector2>().y > 0) 
            {
                move.rotatePart3(rotateSpeed);
            }
            //part3 down
            if (part3.ReadValue<Vector2>().y < 0) 
            {
                move.rotatePart3(rotateSpeed * -1f);
            }


        }
    }
}
