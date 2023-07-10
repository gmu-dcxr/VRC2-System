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
        public GameObject gluebox; // glue box to store the glue

        [Header("Animation")] public string animationString = "Walking";

        private ExperimenterRoutine _routine = ExperimenterRoutine.Default;

        private Animator _animator;

        void Start()
        {
            // clampSizeDict = new Dictionary<int, NetworkPrefabRef>();
            //
            // for (int i = 0; i < clampsTemplate.Count; i++)
            // {
            //     var prefab = clampsTemplate[i];
            //     clampSizeDict.Add(i + 1, prefab);
            // }

            experimenter.stoppingDistance = 0.5f;
            var animator = experimenter.gameObject.GetComponent<Animator>();
            animator.SetBool(animationString, true);
        }

        void MoveToGlueBox()
        {
            Debug.Log("MoveToGlueBox");
            _routine = ExperimenterRoutine.Go;
            experimenter.SetDestination(gluebox.transform.position);
        }

        void RefillGlue()
        {
            Debug.Log("RefillGlue");

            GlobalConstants.currentGlueCapacitiy = GlobalConstants.glueInitialCapacity;
        }

        public void BackToBase()
        {
            Debug.Log("BackToBase");
            experimenter.SetDestination(experimenterBase.transform.position);
        }

        public override void Execute()
        {
            MoveToGlueBox();
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