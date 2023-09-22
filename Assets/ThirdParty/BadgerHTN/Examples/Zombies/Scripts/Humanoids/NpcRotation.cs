using System;
using UnityEngine;
using UnityEngine.AI;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    [Serializable]
    public class NpcRotation
    {
        public NavMeshAgent navAgent;
        public Transform target;
        public float turnSpeed;

        public void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (target == null || navAgent == null)
            {
                return;
            }

            var velocity = navAgent.velocity;
            var navigationDirection = velocity.normalized;
            if (navigationDirection == default)
            {
                return;
            }

            if (navAgent.IsDone())
            {
                return;
            }

            var euler = DirectionHelper.RectangularToEuler(navigationDirection);

            var direction = new Vector3(0, euler.y, 0);
            var quaternion = Quaternion.Euler(direction);

            var speed = turnSpeed * Time.deltaTime * 100f;
            target.rotation = Quaternion.RotateTowards(target.rotation, quaternion, speed);
        }
    }
}