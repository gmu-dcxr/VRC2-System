using UnityEngine.InputSystem;
using UnityEngine;


namespace VRC2.Animations
{
    public class RobotArmInputRecording : BaseInputRecording
    {

        public RoboticArm move;
        public RobotArmInputAction armIA;
        //has to be between 0.00 - 1.00
        public float rotateSpeed = 0.1f;

        private InputAction part0;
        private InputAction part1;
        private InputAction part2;
        private InputAction part3;

        private float angle0 = 0.3f;
        private float angle1 = 0.3f;
        private float angle2 = 0.3f;
        private float angle3 = 0.3f;

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
                angle0 += rotateSpeed;
                move.rotatePart0(angle0);
            }
            //part0 down
            if (part0.ReadValue<Vector2>().y < 0) 
            {
                angle0 -= rotateSpeed;
                move.rotatePart0(angle0);
            }
            //part1 up
            if (part1.ReadValue<Vector2>().y > 0) 
            {
                angle1 += rotateSpeed;
                move.rotatePart1(angle1);
            }
            //part1 down
            if (part1.ReadValue<Vector2>().y < 0)
            {
                angle1 -= rotateSpeed;
                move.rotatePart1(angle1);
            }
            //part2 up
            if (part2.ReadValue<Vector2>().y > 0) 
            {
                angle2 += rotateSpeed;
                move.rotatePart2(angle2);
            }
            //part2 down
            if (part2.ReadValue<Vector2>().y < 0) 
            {
                angle2 -= rotateSpeed;
                move.rotatePart2(angle2);
            }
            //part3 up
            if (part3.ReadValue<Vector2>().y > 0) 
            {
                angle3 += rotateSpeed;
                move.rotatePart3(angle3);
            }
            //part3 down
            if (part3.ReadValue<Vector2>().y < 0) 
            {
                angle3 -= rotateSpeed;
                move.rotatePart3(angle3);
            }


        }
    }
}
