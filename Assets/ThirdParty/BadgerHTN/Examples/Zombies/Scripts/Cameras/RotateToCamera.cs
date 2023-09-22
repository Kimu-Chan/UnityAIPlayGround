using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Cameras
{
    public class RotateToCamera : MonoBehaviour
    {
        private Camera _camera;

        public void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (_camera == null)
            {
                return;
            }

            var dir = transform.position - _camera.transform.position;
            var v = new Vector3(0, dir.y, dir.z);
            transform.rotation = Quaternion.LookRotation(v);
        }

        private void Start()
        {
            _camera = FindObjectOfType<Camera>();
        }
    }
}