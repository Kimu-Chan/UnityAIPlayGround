using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class CharacterAnimation : MonoBehaviour
    {
        public float bounceHeight = 0.5f;
        public AnimationCurve curve;
        public float duration = 1;
        public bool isUpdating = true;
        public float moveThreshold = 0.01f;
        public float progress = -1;
        public Transform target;
        private bool _hasPos;
        private Vector3 _lastPos;
        private Vector3 _start;

        public void Play()
        {
            if (progress >= 0)
            {
                progress += Time.deltaTime / duration;
                if (progress >= 1)
                {
                    progress = -1;
                }

                if (target != null)
                {
                    var c = curve.Evaluate(progress) * bounceHeight;
                    var pos = target.position;
                    pos.y = _start.y + c;
                    target.position = pos;
                }
            }
        }

        protected void Update()
        {
            if (!isUpdating)
            {
                return;
            }

            var delta = Vector3.Distance(_lastPos, transform.position);
            if (_hasPos && delta > moveThreshold)
            {
                if (progress < 0)
                {
                    if (target != null)
                    {
                        _start = target.position;
                    }

                    progress = 0;
                }

                _lastPos = transform.position;
            }

            Play();
            _hasPos = true;
        }
    }
}