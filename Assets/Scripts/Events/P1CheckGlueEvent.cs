using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using VRC2.Agent;

namespace VRC2.Events
{
    public class P1CheckGlueEvent : BaseEvent
    {
        [Header("Settings")] public NavMeshAgent experimenter;
        public GameObject experimenterBase; // where is the experimenter position
        public GameObject stopPoint; // experimenter to stop

        [Header("Animation")] public string animationString = "Walking";
        private Animator animator;

        private ExperimenterRoutine _routine = ExperimenterRoutine.Default;



        void Start()
        {
            animator = experimenter.gameObject.GetComponent<Animator>();

            experimenter.stoppingDistance = 0.5f;
            animator.SetBool(animationString, true);
        }

        void MoveToGlueBox()
        {
            Debug.Log("MoveToGlueBox");
            animator.SetBool(animationString, true);
            _routine = ExperimenterRoutine.Go;
            experimenter.SetDestination(stopPoint.transform.position);
        }

        void RefillGlue()
        {
            Debug.Log("RefillGlue");

            GlobalConstants.currentGlueCapacitiy = GlobalConstants.glueInitialCapacity;
            // sync
            if (Runner != null && Runner.IsRunning)
            {
                RPC_SyncCapacity(GlobalConstants.currentGlueCapacitiy);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SyncCapacity(float cap, RpcInfo info = default)
        {

            if (info.IsInvokeLocal)
            {
                // 
            }
            else
            {
                GlobalConstants.currentGlueCapacitiy = cap;
            }
        }

        public void BackToBase()
        {
            Debug.Log("BackToBase");
            experimenter.SetDestination(experimenterBase.transform.position);
        }

        public override void Execute()
        {
            // MoveToGlueBox();
            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            if (Runner != null && Runner.isActiveAndEnabled && Runner.IsClient)
            {
                // change to P2 
                RPC_SendMessage();
            }
            else
            {
                MoveToGlueBox();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(RpcInfo info = default)
        {
            if (info.IsInvokeLocal)
            {

            }
            else
            {
                MoveToGlueBox();
            }
        }

        private void Update()
        {

            switch (_routine)
            {
                case ExperimenterRoutine.Go:
                    if (AgentHelper.ReachDestination(experimenter))
                    {
                        // fill clamp box
                        RefillGlue();
                        _routine = ExperimenterRoutine.Come;
                        BackToBase();
                    }

                    break;
                case ExperimenterRoutine.Come:
                    if (AgentHelper.ReachDestination(experimenter))
                    {
                        // reach base
                        _routine = ExperimenterRoutine.Default;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}