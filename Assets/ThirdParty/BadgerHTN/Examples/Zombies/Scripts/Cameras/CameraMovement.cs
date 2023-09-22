using System;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Cameras
{
    [Serializable]
    public class CameraMovement
    {
        public Transform self;
        public float smoothSpeed;
        public Transform target;

        public void Update()
        {
            MoveCameraToTarget();
        }

        private void MoveCameraToTarget()
        {
            if (self == null || target == null)
            {
                return;
            }

            self.position = Vector3.Lerp(self.position, target.position, smoothSpeed);
        }
    }
}