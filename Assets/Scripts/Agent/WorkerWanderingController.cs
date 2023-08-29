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

        [Header("Settings")] public float stopDistance = 0.1f;

        public Transform startPoint;
        public Transform endPoint;

        public bool loop = false;

        private Timer _timer;

        private bool startDestination = false;

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

            StartCoroutine("OnCompleteAttackAnimation");
        }

        void MoveToStartPoint()
        {
            startDestination = true;
            _agent.SetDestination(startPoint.position);
        }

        private void Update()
        {
            if (AgentHelper.ReachDestination(_agent))
            {
                // change destination
                if (startDestination)
                {
                    // change to end
                    _agent.SetDestination(endPoint.position);
                    startDestination = false;
                }
                else
                {
                    // change to start
                    _agent.SetDestination(startPoint.position);
                    startDestination = true;
                }
            }
        }

        IEnumerator OnCompleteAttackAnimation()
        {
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                yield return null;

            // TODO: Do something when animation did complete
            print("animation is done");
        }

        void StartTimer(float duration, Action oncomplete)
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(duration, oncomplete, isLooped: false, useRealTime: true);
        }
    }
}