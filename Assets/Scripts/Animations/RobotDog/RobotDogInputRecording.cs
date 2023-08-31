using UnityEngine.InputSystem;
using UnityEngine;

namespace VRC2.Animations
{
    public class RobotDogInputRecording : BaseInputRecording
    {
        [Space(30)] [Header("Body")]
        //Movement
        public float moveSpeed = 1f;

        public float rotateSpeed = 20f;

        //
        public Transform body;

        //Scripts
        public Actions actions;

        //bools
        private bool walk;
        private bool turn;

        private RobotDogInputActions inputActions;
        private InputAction bodyMoveIA;
        private InputAction bodyTurnIA;

        public override void InitInputActions()
        {
            inputActions = new RobotDogInputActions();
            inputActions.Enable();

            bodyMoveIA = inputActions.Body.Move;
            bodyTurnIA = inputActions.Body.Turn;
        }

        public override void DisposeInputActions()
        {
            inputActions.Dispose();
        }

        // Start is called before the first frame update
        void Start()
        {
            walk = false;
            turn = false;
        }

        // Update is called once per frame
        void Update()
        {
            //walking foward
            // if (Input.GetKey(KeyCode.W))
            if (bodyMoveIA.ReadValue<Vector2>().y > 0)
            {
                if (walk == false)
                {
                    actions.Walk();
                    walk = true;
                }

                body.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);

            }

            // turn left
            // if (Input.GetKey(KeyCode.Q))
            if (bodyTurnIA.ReadValue<Vector2>().x < 0)
            {
                if (turn == false)
                {
                    actions.TurnLeft();
                    turn = true;
                }

                body.Rotate(-1 * Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }

            // turn right
            // if (Input.GetKey(KeyCode.E))
            if (bodyTurnIA.ReadValue<Vector2>().x > 0)
            {
                if (turn == false)
                {
                    actions.TurnRight();
                    turn = true;
                }

                body.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }

            // move backwards
            // if (Input.GetKey(KeyCode.S))
            if (bodyMoveIA.ReadValue<Vector2>().y < 0)
            {
                if (walk == false)
                {
                    actions.Walk();
                    walk = true;
                }

                body.Translate(new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime);
            }

            // Strafe left
            // if (Input.GetKey(KeyCode.A))
            if (bodyMoveIA.ReadValue<Vector2>().x < 0)
            {
                if (walk == false)
                {
                    actions.StrafeLeft();
                    walk = true;
                }

                body.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
            } // Strafe right

            // if (Input.GetKey(KeyCode.D))
            if (bodyMoveIA.ReadValue<Vector2>().x > 0)
            {
                if (walk == false)
                {
                    actions.StrafeRight();
                    walk = true;
                }

                body.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
            }


            //No button, go idle
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
                Input.GetKeyUp(KeyCode.A)
                || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
            {
                walk = false;
                turn = false;
                actions.Idle1();
            }

            //Use to look like picking up 
            if (Input.GetKey(KeyCode.R))
            {
                actions.Hit1();
            }

        }
    }
}