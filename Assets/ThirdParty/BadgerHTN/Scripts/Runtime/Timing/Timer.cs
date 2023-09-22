using System;

namespace BadgerHTN
{
    [Serializable]
    public struct Timer
    {
        public float duration;
        private float _timestamp;
        private bool HasBeenSet => _timestamp > 0;

        public bool HasElapsed(float time)
        {
            return !HasBeenSet || time > _timestamp + duration;
        }

        public void Set(float time)
        {
            _timestamp = time;
        }
    }
}