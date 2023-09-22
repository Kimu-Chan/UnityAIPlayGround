using UnityEngine;
using UnityEngine.AI;

namespace BadgerHTN
{
    /*
        Extension class for making it easier to work
        with unity's navmesh agent.
    */
    public static class NavigationExtensions
    {
        /// <summary>
        /// Sets the destination for a navmesh agent. Also sets isStopped=false
        /// since Halt() stops it. 
        /// </summary>
        public static void GoTo(this NavMeshAgent agent, Vector3 destination)
        {
            if (agent == null || !agent.isOnNavMesh || !agent.enabled)
            {
                // Can't set a destination for an invalid agent.
                return;
            }

            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        /// <summary>
        /// Stops the navmesh agent. User will will have to set
        /// isStopped=true for agent to walk again.
        /// </summary>
        public static void Halt(this NavMeshAgent agent)
        {
            if (agent == null || !agent.isOnNavMesh || !agent.enabled)
            {
                return;
            }

            agent.isStopped = true;
        }

        /// <summary>
        /// Check if an navmesh agent has reached it's destination.
        /// Compares against it's pre-defined stopping distance.
        /// </summary>
        public static bool IsDone(this NavMeshAgent agent)
        {
            if (agent == null || !agent.isOnNavMesh || !agent.enabled)
            {
                // Ignore invalid agents.
                return false;
            }

            if (agent.pathPending)
            {
                // Can't be complete if we havn't calculated a path.
                return false;
            }

            // Return true if close enough.
            return agent.remainingDistance <= agent.stoppingDistance;
        }
    }
}