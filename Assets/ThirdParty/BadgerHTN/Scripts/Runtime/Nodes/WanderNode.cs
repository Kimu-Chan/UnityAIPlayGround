using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace BadgerHTN
{
    [NodeMenu("Wander", "Navigation")]
    public class WanderNode : CustomNode<BaseContext>, IDraw<BaseContext>
    {
        private bool _hasStarted;
        private NavMeshAgent _navAgent;
        [Expose] public float radius = 1f;

        public void OnDraw(BaseContext context)
        {
#if UNITY_EDITOR
            if (_navAgent == null)
            {
                return;
            }

            if (!_navAgent.IsDone())
            {
                var p1 = context.Transform.position;

                var v = _navAgent.destination - p1;
                v.y = 0;

                var p2 = p1 + v;

                Handles.color = ColorLib.FromHex("#0e7490");
                Handles.DrawLine(p1, p2, 2);
            }
#endif
        }

        protected override State OnExecute(BaseContext context)
        {
            var transform = context.Transform;
            _navAgent = transform?.GetComponent<NavMeshAgent>();
            if (_navAgent == null)
            {
                // Abort if no NavAgent attached to agent gameObject
                Debug.LogError($"WanderNode requires a NavMeshAgent attached to the agent gameObject");
                return State.Failed;
            }

            if (!_hasStarted)
            {
                // Pick a random point on the navmesh and go to it
                if (RandomPoint(transform.position, out var result))
                {
                    _navAgent.GoTo(result);
                }
                else
                {
                    return State.Failed;
                }

                _hasStarted = true;
            }

            if (_navAgent.IsDone())
            {
                // Set success if agent has reached destination
                return State.Success;
            }

            return State.Running;
        }

        protected override void OnInterrupt(BaseContext context)
        {
            _navAgent?.Halt();
        }

        private bool RandomPoint(Vector3 center, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * radius;
                if (!NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
                {
                    continue;
                }

                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }
    }
}