using UnityEngine;

namespace BadgerHTN
{
    public class Actor : MonoBehaviour
    {
        public IAgentComponent agentComponent;
        public Combat combat;
        public Faction faction;
        public Health health;
        public IAgent Agent => agentComponent?.Agent;
        public Vector3 Position => transform.position;

        public void ChangeHealth(int value, CombatInfo info = default)
        {
            var didChange = health.Change(value);
            if (didChange)
            {
                ActorEvents.HealthChanged?.Invoke(this, info);
            }
        }

        private void Awake()
        {
            agentComponent = GetComponent<IAgentComponent>();
        }
    }
}