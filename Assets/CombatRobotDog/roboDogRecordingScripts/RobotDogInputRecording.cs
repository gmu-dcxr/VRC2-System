using UnityEngine.InputSystem;
using UnityEngine;

namespace VRC2.Animations
{
    public class RobotDogInputRecording : BaseInputRecording
    {
        private CombatDogInputActions dogIA;
        public CombatDogController dogCtr;
        private InputAction walkInput;
        private InputAction turnInput;
        private InputAction pickUpInput;

        private bool walk = false;
        private bool turn = false;
        private bool idle = false;
        private float moveSpeed { get => dogCtr.moveSpeed; }
        private float rotateSpeed { get => dogCtr.rotateSpeed; }
        public Actions act;
        public GameObject dog;

      
        public override void InitInputActions() 
        {
            dogIA = new CombatDogInputActions();
            dogIA.Enable();

            walkInput = dogIA.CombatDog.Walk;
            turnInput = dogIA.CombatDog.Turn;
            pickUpInput = dogIA.CombatDog.Hit;
        }

        public override void DisposeInputActions() 
        {
            dogIA.Dispose();
        }

        public override void UpdateLogic() 
        {
           var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //walking foward
            // if (Input.GetKey(KeyCode.W))
            if (walkInput.ReadValue<Vector2>().y > 0)
            {
                if (walk == false)
                {
                    act.Walk();
                    walk = true;
                    idle = false;
                }
                dog.transform.Translate(new Vector3(0,0,1) * moveSpeed * Time.deltaTime);

            }
            // turn left
            //if (Input.GetKey(KeyCode.Q))
            else if (turnInput.ReadValue<Vector2>().x < 0)
            {
                if (turn == false)
                {
                    act.TurnLeft();
                    turn = true;
                    idle = false;
                }
                dog.transform.Rotate(-1 * Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }
            // turn right
            //if (Input.GetKey(KeyCode.E))
            else if (turnInput.ReadValue<Vector2>().x > 0)
            {
                if (turn == false)
                {
                    act.TurnRight();
                    turn = true;
                    idle = false;
                }
                dog.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }
            // move backwards
            //if (Input.GetKey(KeyCode.S))
            else if (walkInput.ReadValue<Vector2>().y < 0)
            {
                if (walk == false)
                {
                    act.Walk();
                    walk = true;
                    idle = false;
                }
                dog.transform.Translate(new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime);
            }
            // Strafe left
            //if (Input.GetKey(KeyCode.A))
            else if (walkInput.ReadValue<Vector2>().x < 0)
            {
                if (walk == false)
                {
                    act.StrafeLeft();
                    walk = true; 
                    idle = false;
                }
                dog.transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
            }// Strafe right
             // if (Input.GetKey(KeyCode.D))
            else if (walkInput.ReadValue<Vector2>().x > 0)
            {

                if (walk == false)
                {
                    act.StrafeRight();
                    walk = true;
                    idle = false;
                }
                dog.transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
            }

            //Use to look like picking up 
            //if (Input.GetKey(KeyCode.R))
            else if (pickUpInput.triggered)
            {
                act.Hit1();
            }
            else
            {
                if (idle == false) { 
                    act.Idle1();
                }
                idle = true;
                walk = false;
                turn = false;
            }            
        }
    }
}
