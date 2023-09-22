using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Sensors
{
    public class TargetSensor : MonoBehaviour
    {
        private IAgentComponent _agentComponent;
        private SphereCollider _sphereCollider;

        public void Awake()
        {
            _agentComponent = GetComponentInParent<IAgentComponent>();
            _sphereCollider = GetComponent<SphereCollider>();
        }

        public void Update()
        {
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            var blackboard = _agentComponent?.Agent.Blackboard;
            if (blackboard == null)
            {
                return;
            }

            var radius = _sphereCollider.radius;

            bool isTargetNear = false;

            var targetGameObject = blackboard.Get<GameObject>("Target");
            if (targetGameObject != null)
            {
                var distance = Vector3.Distance(targetGameObject.transform.position,
                    _agentComponent.Transform.position);
                isTargetNear = distance < radius;
            }

            blackboard.Set<bool>("IsTargetNear", isTargetNear);
        }
    }
}