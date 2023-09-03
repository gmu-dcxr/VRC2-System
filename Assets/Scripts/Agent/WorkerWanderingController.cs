using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityTimer;

namespace VRC2.Agent
{
    public class WorkerWanderingController : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private Animator _animator;

        [Space(30)] [Header("Settings")] public float stopDistance = 0.1f;

        public Transform startPoint;
        public Transform endPoint;

        public float startToEndYRotation = 0;
        public float endToStartYRotation = 0;

        private float currentYRotation = 0;

        public bool loop = false;

        [Space(30)] [Header("Animation")] public string idle;

        [Space(30)] [Header("Time")] public float idleTime = 2.0f;
        public float workingTime = 5.0f;

        // private Timer _timer;
        private Timer _idleTimer;
        private Timer _workingTimer;

        private bool startDestination = false;

        private bool procceeded = false;

        private void Start()
        {
            _agent = gameObject.GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                _agent = gameObject.AddComponent<NavMeshAgent>();
            }

            _agent.stoppingDistance = stopDistance;

            _animator = gameObject.GetComponent<Animator>();

            MoveToStartPoint();
        }

        void MoveToStartPoint()
        {
            startDestination = true;
            _agent.SetDestination(startPoint.position);
            StartWalking();

            currentYRotation = startToEndYRotation;
        }

        void StartWalking()
        {

        }

        void StartIdle()
        {
            _animator.SetBool(idle, true);
        }

        void StartWorking()
        {
            _animator.SetBool(idle, false);
        }

        void StartAnimation(Vector3 destination)
        {
            StartIdle();
            StartTimer(ref _idleTimer, idleTime, () =>
            {
                StartWorking();

                StartTimer(ref _workingTimer, workingTime, () =>
                {
                    // should start walking
                    StartWalking();

                    // change destination
                    _agent.SetDestination(destination);
                    startDestination = !startDestination;
                    procceeded = false;
                    if (startDestination)
                    {
                        currentYRotation = startToEndYRotation;
                    } else
                    {
                        currentYRotation = endToStartYRotation;
                    }
                });
            });
        }

        private void Update()
        {
            if (AgentHelper.ReachDestination(_agent) && !procceeded)
            {
                procceeded = true;
                // change destination
                if (startDestination)
                {
                    StartAnimation(endPoint.position);
                }
                else
                {
                    StartAnimation(startPoint.position);
                }
            }
            //
            gameObject.transform.localRotation = Quaternion.Euler(0, currentYRotation, 0);
        }

        void StartTimer(ref Timer timer, float duration, Action oncomplete)
        {
            if (timer != null)
            {
                Timer.Cancel(timer);
            }

            timer = Timer.Register(duration, oncomplete, isLooped: false, useRealTime: true);
        }
    }
}