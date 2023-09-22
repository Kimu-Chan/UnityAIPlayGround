using UnityEngine;

namespace BadgerHTN
{
    public abstract class AgentComponent<T> : MonoBehaviour, IAgentComponent
        where T : IAgent, new()
    {
        [SerializeField] private T agent;
        [field: SerializeField] public bool AsyncSearch { get; set; }
        [field: SerializeField] public bool AutoTick { get; set; } = true;

        /// <summary>
        /// Create a new agent on start and assign it a name, asset
        /// and a gameobject.
        /// </summary>
        private void Awake()
        {
            agent = new T();
            agent.AsyncSearch = AsyncSearch;
            agent.SetName(gameObject.name);
            agent.SetAsset(Asset);
            agent.SetGameObject(gameObject);
        }

        /// <summary>
        /// Update agent.
        /// </summary>
        private void Update()
        {
            if (Agent == null)
            {
                return;
            }

            if (AutoTick)
            {
                // Tick agent if AutoTick is enabled in inspector view.
                // Can also be called manually.
                Agent.Tick();
            }
        }

        private void OnDrawGizmos()
        {
            agent?.OnDraw();
        }

        private void OnDrawGizmosSelected()
        {
            agent?.OnDrawSelected();
        }

        public IAgent Agent => agent;
        [field: SerializeField] public GraphAsset Asset { get; set; }
        public Transform Transform => transform;

        [Button]
        public void Tick()
        {
            agent?.Tick();
        }
    }
}