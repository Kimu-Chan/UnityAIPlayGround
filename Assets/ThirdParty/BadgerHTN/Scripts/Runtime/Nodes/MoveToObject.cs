using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace BadgerHTN
{
    [NodeMenu("MoveToObject", "Navigation")]
    public class MoveToObjectNode : CustomNode<BaseContext>, IDraw<BaseContext>
    {
        private GameObject _target;
        [Blackboard(typeof(GameObject))] public string gameObject;

        [Expose] public float threshold = 0.2f;

        public void OnDraw(BaseContext context)
        {
#if UNITY_EDITOR
            if (_target == null)
            {
                return;
            }

            if (context.Transform != null && _target != null)
            {
                var p1 = context.Transform.position;

                var v = (_target.transform.position - p1);
                v.y = 0;

                var p2 = p1 + v;

                Handles.color = ColorLib.FromHex("#0e7490");
                Handles.DrawLine(p1, p2, 2f);
            }
#endif
        }

        protected override void OnInterrupt(BaseContext context)
        {
            // Stop navmesh agent if the node was interrupted.
            var navAgent = context?.Transform?.GetComponent<NavMeshAgent>();
            navAgent?.Halt();
        }

        protected override State OnExecute(BaseContext context)
        {
            if (string.IsNullOrEmpty(gameObject))
            {
                return State.Failed;
            }

            // Set target
            _target = context?.Blackboard.Get<GameObject>(gameObject);
            if (_target == null)
            {
                // Abort and stop navAgent if no target
                Halt(context);
                return State.Failed;
            }

            var navAgent = context?.Transform?.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                // Fail if no navAgent attached
                Debug.LogWarning($"MoveToPlayerNode requires a NavMeshAgent attached to the agent gameObject");
                return State.Failed;
            }

            // Update destination
            if (navAgent.destination != _target.transform.position)
            {
                var delta = Vector3.Distance(navAgent.destination, _target.transform.position);
                if (delta > threshold)
                {
                    // Set destination if not already close enough to target
                    navAgent.GoTo(_target.transform.position);
                }
            }

            if (navAgent.IsDone())
            {
                // Set success if we have reached the target
                return State.Success;
            }

            return State.Running;
        }

        private static void Halt(BaseContext context)
        {
            // Stop the navmesh agent.
            var navAgent = context?.Transform?.GetComponent<NavMeshAgent>();
            navAgent?.Halt();
        }
    }
}