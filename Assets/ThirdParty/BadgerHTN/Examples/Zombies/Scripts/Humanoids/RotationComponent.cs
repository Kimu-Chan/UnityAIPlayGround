using UnityEngine;
using UnityEngine.AI;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class RotationComponent : MonoBehaviour, IRotateTo
    {
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] private Transform transformToRotate;

        private NavMeshAgent _navMeshAgent;
        private Vector3 _targetRotation;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Update()
        {
            UpdateNavMeshAgent();
            UpdateRotation();
        }

        public void RotateTo(Vector3 target)
        {
            _targetRotation = target;
        }

        public void RotateTo(GameObject target)
        {
            var dir = (target.transform.position - transformToRotate.position).normalized;
            RotateTo(dir);
        }

        public bool IsDone()
        {
            if (transformToRotate == null)
            {
                return false;
            }
            return transformToRotate.rotation == GetTargetDirection();
        }

        private void UpdateNavMeshAgent()
        {
            if (_navMeshAgent == null)
            {
                return;
            }

            if (!_navMeshAgent.IsDone())
            {
                RotateTo(_navMeshAgent.velocity.normalized);
            }
        }


        private void UpdateRotation()
        {
            var quaternion = GetTargetDirection();

            var speed = turnSpeed * Time.deltaTime * 100f;
            transformToRotate.rotation = Quaternion.RotateTowards(transformToRotate.rotation, quaternion, speed);
        }

        private Quaternion GetTargetDirection()
        {
            var euler = DirectionHelper.RectangularToEuler(_targetRotation.normalized);

            var direction = new Vector3(0, euler.y, 0);
            var quaternion = Quaternion.Euler(direction);

            return quaternion;
        }
    }
}