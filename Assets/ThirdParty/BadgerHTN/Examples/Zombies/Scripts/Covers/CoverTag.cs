using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Covers
{
    public class CoverTag : MonoBehaviour
    {
        public IAgent agent;
        public bool valid = true;

        private void OnDisable()
        {
            valid = false;
            if (agent != null)
            {
                agent.Blackboard.Set<bool>("HasCover", false);
                agent.Blackboard.Set<GameObject>("Cover", null);
            }
        }
    }
}