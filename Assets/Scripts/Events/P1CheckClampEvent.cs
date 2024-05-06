using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;

using AgentHelper = VRC2.Agent.AgentHelper;

namespace VRC2.Events
{
    enum ExperimenterRoutine
    {
        Default = 0,
        Go = 1,
        Come = 2,
    }

    public class P1CheckClampEvent : BaseEvent
    {
        [Header("Settings")] public NavMeshAgent experimenter;
        public GameObject experimenterBase; // where is the experimenter position
        public GameObject stopPoint; // experimenter to stop
        public GameObject dropPoint; // clamp to drop from
        public int amount = 1; // how many to refill for each size of clamp

        [Header("Clamp Prefabs")] public List<NetworkPrefabRef> clampsTemplate;

        [Header("Animation")] public string animationString = "Walking";
        private Animator animator;

        // private IDictionary<int, NetworkPrefabRef> clampSizeDict;

        private ExperimenterRoutine _routine = ExperimenterRoutine.Default;


        void Start()
        {
            // clampSizeDict = new Dictionary<int, NetworkPrefabRef>();
            //
            // for (int i = 0; i < clampsTemplate.Count; i++)
            // {
            //     var prefab = clampsTemplate[i];
            //     clampSizeDict.Add(i + 1, prefab);
            // }

            animator = experimenter.gameObject.GetComponent<Animator>();

            experimenter.stoppingDistance = 0.5f;
            animator.SetBool(animationString, true);
        }

        void MoveToClampBox()
        {
            Debug.Log("MoveToClampBox");
            animator.SetBool(animationString, true);
            _routine = ExperimenterRoutine.Go;
            experimenter.SetDestination(stopPoint.transform.position);
        }

        void RefillClamp()
        {
            Debug.Log("RefillClamp");
            // GlobalConstants.currentClampCount = GlobalConstants.clampInitialCount;

            var runner = GlobalConstants.networkRunner;
            var localPlayer = GlobalConstants.localPlayer;

            var pos = dropPoint.transform.position;

            for (int i = 0; i < amount; i++)
            {
                foreach (var prefab in clampsTemplate)
                {
                    var spawned = runner.Spawn(prefab, pos, Quaternion.identity, localPlayer);
                }
            }
        }

        public void BackToBase()
        {
            Debug.Log("BackToBase");
            experimenter.SetDestination(experimenterBase.transform.position);
        }

        public override void Execute()
        {
            MoveToClampBox();
        }

        private void Update()
        {

            switch (_routine)
            {
                case ExperimenterRoutine.Go:
                    if (AgentHelper.ReachDestination(experimenter))
                    {
                        // fill clamp box
                        RefillClamp();
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