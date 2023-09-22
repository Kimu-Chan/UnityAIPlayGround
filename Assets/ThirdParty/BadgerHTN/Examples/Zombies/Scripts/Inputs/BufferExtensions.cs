using System.Linq;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Inputs
{
    public static class BufferExtensions
    {
        public static Vector3 GetVector(this Buffer<Vector3> buffer, int size = 0)
        {
            if (buffer == null)
            {
                return default;
            }

            if (size <= 0)
            {
                size = buffer.Count;
            }

            var elements = buffer.LastElements(size);
            return elements.Aggregate(Vector3.zero, (current, element) => current + element);
        }
    }
}