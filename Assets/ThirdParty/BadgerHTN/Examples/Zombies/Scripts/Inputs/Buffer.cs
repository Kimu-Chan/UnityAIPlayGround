using System;
using System.Collections.Generic;
using System.Linq;

namespace BadgerHTN.Examples.Zombies.Scripts.Inputs
{
    public class Buffer<T> : List<T>
    {
        public readonly int capacity;

        public Buffer(int capacity)
        {
            this.capacity = capacity;
        }

        public Buffer()
        {
        }

        public new void Add(T v)
        {
            base.Add(v);
            if (capacity > 0 && Count > capacity && Count > 0)
            {
                RemoveAt(0);
            }
        }

        public IEnumerable<T> LastElements(int size)
        {
            return this.Skip(Math.Max(0, Count - size));
        }
    }
}