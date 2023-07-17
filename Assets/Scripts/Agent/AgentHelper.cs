using UnityEngine;
using UnityEngine.AI;

namespace VRC2.Agent
{
    public static class AgentHelper
    {
        #region Nav Agent

        public static bool ReachDestination(NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}