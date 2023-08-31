using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRC2.Animations.RobotDog
{

    public class RobotDogInputRecording : BaseInputRecording
    {

        private RobotDogInputActions inputActions;

        private InputAction bodyMoveIA;
        private InputAction bodyTurnIA;

        #region Body

        public CombatDogController dogCtr;

        private bool walk = false;
        private bool turn = false;
        private bool idle = false;

        private float moveSpeed
        {
            get => dogCtr.moveSpeed;
        }

        private float rotateSpeed
        {
            get => dogCtr.rotateSpeed;
        }

        public Actions act;
        public GameObject dog;

        #endregion


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

        // public virtual void UpdateLogic()
        // {
        //     // var input = m_MoveInput.ReadValue<Vector2>();
        //     // var movement = new Vector3(input.x,0f,input.y) * 4f * Time.deltaTime;
        //     // m_Player.Translate(movement,Space.Self);
        //     
        //     ControlBody();
        // }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ControlBody();
        }

        #region Robot dog body control

        void ControlBody()
        {
            //walking foward
            if (bodyMoveIA.ReadValue<Vector2>().y > 0)
            {
                if (walk == false)
                {
                    act.Walk();
                    walk = true;
                    idle = false;
                }

                dog.transform.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);

            }
            // turn left
            else if (bodyTurnIA.ReadValue<Vector2>().x < 0)
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
            else if (bodyTurnIA.ReadValue<Vector2>().x > 0)
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
            else if (bodyMoveIA.ReadValue<Vector2>().y < 0)
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
            else if (bodyMoveIA.ReadValue<Vector2>().x < 0)
            {
                if (walk == false)
                {
                    act.StrafeLeft();
                    walk = true;
                    idle = false;
                }

                dog.transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
            } // Strafe right
            else if (bodyMoveIA.ReadValue<Vector2>().x > 0)
            {

                if (walk == false)
                {
                    act.StrafeRight();
                    walk = true;
                    idle = false;
                }

                dog.transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
            }
        }

        #endregion

        #region Robot dog arm control



        #endregion
    }
}