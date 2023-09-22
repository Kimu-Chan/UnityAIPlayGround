using UnityEngine;

namespace BadgerHTN.Examples.Guides._02_Chase
{
    public class TargetCheck : MonoBehaviour
    {
        public GameObject target;
        public float range = 2f;
        private BadgerComponent _agent;

        private void Awake()
        {
            _agent = GetComponent<BadgerComponent>();
        }

        private void Update()
        {
            var blackboard = _agent.Agent.Blackboard;
            if (target == null)
            {
                // Exit if not assigned or missing reference exception.
                blackboard.Set<GameObject>("Target", null);
                return;
            }

            // Distance to target GameObject.
            var distance = Vector3.Distance(transform.position, target.transform.position);
            bool inRange = distance <= range;

            // Set target if in range, otherwise null.
            blackboard.Set<GameObject>("Target", inRange ? target : null);
        }
    }
}