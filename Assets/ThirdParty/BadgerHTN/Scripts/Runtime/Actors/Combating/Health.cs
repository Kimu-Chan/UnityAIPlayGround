using System;

namespace BadgerHTN
{
    [Serializable]
    public class Health
    {
        public int hitPoints;
        public int maxHitPoints;
        public bool IsAlive => !IsDead;
        public bool IsDead => hitPoints <= 0;

        public bool Change(int value)
        {
            var tmp = hitPoints;

            hitPoints += value;
            Clamp();

            var didChange = tmp != hitPoints;
            return didChange;
        }

        public bool Set(int value)
        {
            var tmp = hitPoints;

            hitPoints = value;
            Clamp();

            return tmp != hitPoints;
        }

        private void Clamp()
        {
            hitPoints = Math.Clamp(hitPoints, 0, maxHitPoints);
        }
    }
}