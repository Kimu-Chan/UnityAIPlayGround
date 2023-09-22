using System;
using BadgerHTN.Examples.Zombies.Scripts.Inputs;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    [Serializable]
    public class PlayerRotation
    {
        public Buffer<Vector3> buffer;
        public Transform target;
        public float turnSpeed;

        public PlayerRotation()
        {
            buffer = new Buffer<Vector3>(10);
        }

        public void Update()
        {
            AddInputToBuffer();
            Rotate();
        }

        private void AddInputToBuffer()
        {
            var dir = DirectionHelper.GetInputDirection();
            if (dir != default)
            {
                buffer.Add(dir);
            }
        }

        private void Rotate()
        {
            if (target == null)
            {
                return;
            }

            int size = buffer.capacity;
            var sum = buffer.GetVector(size);
            var average = sum / size;

            var euler = DirectionHelper.RectangularToEuler(average);

            var direction = new Vector3(0, euler.y, 0);
            var quaternion = Quaternion.Euler(direction);

            var speed = turnSpeed * Time.deltaTime * 100f;
            target.rotation = Quaternion.RotateTowards(target.rotation, quaternion, speed);
        }
    }
}