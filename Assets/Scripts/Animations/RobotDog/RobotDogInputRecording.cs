using System;
using System.Diagnostics;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine;
using VRC2.Events;

namespace VRC2.Animations
{
    public class RobotDogInputRecording : BaseInputRecording
    {

        [Space(30)] public string filename = "robotdogpickup";

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

        [Space(30)] [Header("Arm")] public RoboticArm arm;
        public float armRotateSpeed = 0.01f;

        private float gripSpeed = 0.5f;
        private float grip = 0.0f;

        private float angle0 = 0.0f;
        private float angle1 = 0.0f;
        private float angle2 = 0.0f;
        private float angle3 = 0.25f; // FIX: default is 0.25

        private RobotDogInputActions inputActions;
        private InputAction bodyMoveIA;
        private InputAction bodyTurnIA;
        private InputAction stopIA;
        private InputAction d0IA;
        private InputAction d1IA;
        private InputAction d2IA;
        private InputAction d3IA;
        private InputAction gripIA;

        public System.Action OnCloseGripOnce;
        public System.Action OnNeedReleasingOnce;

        private bool onCloseGripped = false;
        private bool onNeedReleasing = false;

        [HideInInspector] public bool forceStop = false;

        public override void InitInputActions()
        {
            inputActions = new RobotDogInputActions();
            inputActions.Enable();

            bodyMoveIA = inputActions.Body.Move;
            bodyTurnIA = inputActions.Body.Turn;
            stopIA = inputActions.Body.Stop;

            d0IA = inputActions.Arm.DOF0;
            d1IA = inputActions.Arm.DOF1;
            d2IA = inputActions.Arm.DOF2;
            d3IA = inputActions.Arm.DOF3;
            gripIA = inputActions.Arm.Grip;
        }

        public override void DisposeInputActions()
        {
            inputActions.Dispose();
        }

        public override string GetFilename()
        {
            return filename;
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
            // if(Runner == null || !Runner.isActiveAndEnabled) return;

            ControlRobotBody();
            ControlRobotArm();
        }

        #region Robot body control

        #region RPC actions

        private void InvokeAction_Impl(int index)
        {
            switch (index)
            {
                case 0:
                    actions.Walk();
                    walk = true;
                    break;
                case 1:
                    actions.TurnLeft();
                    turn = true;
                    break;
                case 2:
                    actions.TurnRight();
                    turn = true;
                    break;
                case 3:
                    actions.Walk();
                    walk = true;
                    break;
                case 4:
                    actions.StrafeLeft();
                    walk = true;
                    break;
                case 5:
                    actions.StrafeRight();
                    walk = true;
                    break;
                case 6:
                    walk = false;
                    turn = false;
                    actions.Idle1();
                    break;
                default:
                    break;
            }
        }

        private void RPC_Invoke(int index)
        {
            if (Runner == null || !Runner.isActiveAndEnabled)
            {
                // run locally
                InvokeAction_Impl(index);
            }
            else
            {
                RPC_Invoke_Impl(index);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_Invoke_Impl(int index, RpcInfo info = default)
        {
            InvokeAction_Impl(index);
        }



        #endregion

        void ControlRobotBody()
        {
            //walking foward
            // if (Input.GetKey(KeyCode.W))
            if (bodyMoveIA.ReadValue<Vector2>().y > 0)
            {
                if (walk == false)
                {
                    // actions.Walk();
                    // walk = true;  
                    RPC_Invoke(0);
                }

                body.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);

            }

            // turn left
            // if (Input.GetKey(KeyCode.Q))
            if (bodyTurnIA.ReadValue<Vector2>().x < 0)
            {
                if (turn == false)
                {
                    // actions.TurnLeft();
                    // turn = true;
                    //
                    RPC_Invoke(1);
                }

                body.Rotate(-1 * Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }

            // turn right
            // if (Input.GetKey(KeyCode.E))
            if (bodyTurnIA.ReadValue<Vector2>().x > 0)
            {
                if (turn == false)
                {
                    // actions.TurnRight();
                    // turn = true;

                    RPC_Invoke(2);
                }

                body.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
            }

            // move backwards
            // if (Input.GetKey(KeyCode.S))
            if (bodyMoveIA.ReadValue<Vector2>().y < 0)
            {
                if (walk == false)
                {
                    // actions.Walk();
                    // walk = true;
                    RPC_Invoke(3);
                }

                body.Translate(new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime);
            }

            // Strafe left
            // if (Input.GetKey(KeyCode.A))
            if (bodyMoveIA.ReadValue<Vector2>().x < 0)
            {
                if (walk == false)
                {
                    // actions.StrafeLeft();
                    // walk = true;
                    RPC_Invoke(4);

                }

                body.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
            } // Strafe right

            // if (Input.GetKey(KeyCode.D))
            if (bodyMoveIA.ReadValue<Vector2>().x > 0)
            {
                if (walk == false)
                {
                    // actions.StrafeRight();
                    // walk = true;

                    RPC_Invoke(5);
                }

                body.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
            }

            if (stopIA.triggered)
            {
                // walk = false;
                // turn = false;
                // actions.Idle1();

                RPC_Invoke(6);
            }

            //No button, go idle
            // if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) ||
            //     Input.GetKeyUp(KeyCode.A)
            //     || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
            //
            // //Use to look like picking up 
            // if (Input.GetKey(KeyCode.R))
            // {
            //     actions.Hit1();
            // }
        }

        public bool IsIdle()
        {
            return actions.IsIdle();
        }

        #endregion

        #region RPC actions to change to arm

        private void RotatePart_Impl(int index, float angle)
        {
            switch (index)
            {
                case 0:
                    arm.rotatePart0(angle);
                    break;
                case 1:
                    arm.rotatePart1(angle);
                    break;
                case 2:
                    arm.rotatePart2(angle);
                    break;
                case 3:
                    arm.rotatePart3(angle);
                    break;
            }
        }

        private void RPC_RotatePart(int index, float angle)
        {
            if (Runner == null || !Runner.isActiveAndEnabled)
            {
                // run locally
                RotatePart_Impl(index, angle);
            }
            else
            {
                RPC_RotatePart_Impl(index, angle);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_RotatePart_Impl(int index, float angle, RpcInfo info = default)
        {
            RotatePart_Impl(index, angle);
        }

        private void OperateGrip_Impl(bool open, float speed)
        {
            if (open)
            {
                arm.OpenGrip(speed);
            }
            else
            {
                arm.CloseGrip(speed);
            }
        }

        private void RPC_OperateGrip(bool open, float speed)
        {
            if (Runner == null || !Runner.isActiveAndEnabled)
            {
                // run locally
                OperateGrip_Impl(open, speed);
            }
            else
            {
                RPC_OperateGrip_Impl(open, speed);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_OperateGrip_Impl(bool open, float speed, RpcInfo info = default)
        {
            OperateGrip_Impl(open, speed);
        }

        private void ResetArm_Impl()
        {
            angle0 = 0.0f;
            angle1 = 0.0f;
            angle2 = 0.0f;
            angle3 = 0.25f;
            arm.ResetRotations();
        }

        private void RPC_ResetArm()
        {
            if (Runner == null || !Runner.isActiveAndEnabled)
            {
                // run locally
                ResetArm_Impl();
            }
            else
            {
                RPC_ResetArm_Impl();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ResetArm_Impl(RpcInfo info = default)
        {
            angle0 = 0.0f;
            angle1 = 0.0f;
            angle2 = 0.0f;
            angle3 = 0.25f;
            arm.ResetRotations();
        }


        #endregion


        #region Control robot arm

        void ControlRobotArm()
        {
            //part0 up
            if (d0IA.ReadValue<Vector2>().x > 0)
            {
                angle0 += armRotateSpeed;
                // arm.rotatePart0(angle0);
                RPC_RotatePart(0, angle0);
            }

            //part0 down
            if (d0IA.ReadValue<Vector2>().x < 0)
            {
                angle0 -= armRotateSpeed;
                // arm.rotatePart0(angle0);
                RPC_RotatePart(0, angle0);
            }

            //part1 up
            if (d1IA.ReadValue<Vector2>().x > 0)
            {
                angle1 += armRotateSpeed;
                // arm.rotatePart1(angle1);
                RPC_RotatePart(1, angle1);
            }

            //part1 down
            if (d1IA.ReadValue<Vector2>().x < 0)
            {
                angle1 -= armRotateSpeed;
                // arm.rotatePart1(angle1);
                RPC_RotatePart(1, angle1);
            }

            //part2 up
            if (d2IA.ReadValue<Vector2>().x > 0)
            {
                angle2 += armRotateSpeed;
                // arm.rotatePart2(angle2);
                RPC_RotatePart(2, angle2);
            }

            //part2 down
            if (d2IA.ReadValue<Vector2>().x < 0)
            {
                angle2 -= armRotateSpeed;
                // arm.rotatePart2(angle2);
                RPC_RotatePart(2, angle2);
            }

            //part3 up
            if (d3IA.ReadValue<Vector2>().x > 0)
            {
                angle3 += armRotateSpeed;
                // arm.rotatePart3(angle3);
                RPC_RotatePart(3, angle3);
            }

            //part3 down
            if (d3IA.ReadValue<Vector2>().x < 0)
            {
                angle3 -= armRotateSpeed;
                // arm.rotatePart3(angle3);
                RPC_RotatePart(3, angle3);
            }

            // grip
            if (gripIA.ReadValue<Vector2>().x < 0)
            {
                onNeedReleasing = false;
                if (!onCloseGripped && OnCloseGripOnce != null)
                {
                    onCloseGripped = true;
                    OnCloseGripOnce();
                }

                // arm.CloseGrip(gripSpeed);
                RPC_OperateGrip(false, gripSpeed);
            }

            if (gripIA.ReadValue<Vector2>().x > 0)
            {
                onCloseGripped = false;
                // arm.OpenGrip(gripSpeed);
                RPC_OperateGrip(true, gripSpeed);
                if (!onNeedReleasing && arm.needReleasing && OnNeedReleasingOnce != null)
                {
                    onNeedReleasing = true;
                    OnNeedReleasingOnce();
                }
            }
        }

        public void ResetArm()
        {
            // angle0 = 0.0f;
            // angle1 = 0.0f;
            // angle2 = 0.0f;
            // angle3 = 0.25f;
            // arm.ResetRotations();
            RPC_ResetArm();
        }


        #endregion
    }
}