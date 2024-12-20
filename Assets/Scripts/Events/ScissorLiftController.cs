﻿using System;
using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class ScissorLiftController : NetworkBehaviour
    {
        [Header("Speed")] public float upDown = 1.0f;
        public float leftRight = 0.1f;
        private Animator animator;

        private ScissorLiftEnterExit _scissorLiftEnterExit;

        private NetworkObject _networkObject;

        private NetworkRunner _runner => _networkObject.Runner;

        private bool resetting = false;
        
        // P2
        public bool IsClient => _runner != null && _runner.IsClient;

        private void Start()
        {
            animator = GetComponent<Animator>();

            _scissorLiftEnterExit = GetComponentInChildren<ScissorLiftEnterExit>();
            // set controller
            _scissorLiftEnterExit.controller = this;

            _scissorLiftEnterExit.OnExitLift += OnExitLift;

            _networkObject = GetComponent<NetworkObject>();
        }

        private void OnExitLift()
        {
            // auto reset the lifter
            resetting = true;
            if (_runner != null && _runner.IsRunning)
            {
                RPC_Reset();
            }
            else
            {
                ProcessReset();
            }
        }

        void PlayAnimator(float speed)
        {
            animator.speed = speed;
            animator.Play("move", -1, float.NegativeInfinity);
        }

        void StopAnimator()
        {
            animator.speed = 0;
        }


        void Up(float speed)
        {
            animator.SetFloat("Direction", 1);
            PlayAnimator(speed);
        }

        void Down(float speed)
        {
            animator.SetFloat("Direction", -1);
            PlayAnimator(speed);
        }

        void Left(float speed)
        {
            transform.Translate(0, 0, -speed * Time.deltaTime, Space.Self);
        }

        void Right(float speed)
        {
            transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        }

        bool ReachTop()
        {
            var frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return frame > 1.0f;
        }

        bool ReachBottom()
        {
            var frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return frame < 0.0f;
        }

        #region Use RPC to sync animation

        void ProcessInput(float horizontal, float vertical)
        {
            if (vertical > 0)
            {
                if (!ReachTop())
                {
                    Up(upDown);
                }
                else
                {
                    StopAnimator();
                }
            }
            else if (vertical < 0)
            {
                if (!ReachBottom())
                {
                    Down(upDown);
                }
                else
                {
                    StopAnimator();
                }
            }
            else if (horizontal < 0)
            {
                Left(leftRight);
            }
            else if (horizontal > 0)
            {
                Right(leftRight);
            }
            else
            {
                StopAnimator();
            }
        }

        void ProcessReset()
        {
            resetting = true;
        }


        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(float horizontal, float vertical, RpcInfo info = default)
        {
            ProcessInput(horizontal, vertical);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_Reset(RpcInfo info = default)
        {
            ProcessReset();
        }

        #endregion

        private void Update()
        {
            // reset when resetting
            if (resetting)
            {
                if (!ReachBottom())
                {
                    Down(upDown);
                }
                else
                {
                    resetting = false;
                    StopAnimator();
                    _scissorLiftEnterExit.onResetting = false;
                }

                return;
            }

            // return if not host
            if(IsClient) return;
            
            // return if not entered
            if (!_scissorLiftEnterExit.Entered) return;

            // ovr input
            float horizontalInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).x;
            float verticalInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).y;

            if (Math.Abs(horizontalInput) > Math.Abs(verticalInput))
            {
                verticalInput = 0;
            }
            else
            {
                horizontalInput = 0;
            }

            if (_runner != null && _runner.IsRunning)
            {
                RPC_SendMessage(horizontalInput, verticalInput);
            }
            else
            {
                ProcessInput(horizontalInput, verticalInput);
            }

        }
    }
}