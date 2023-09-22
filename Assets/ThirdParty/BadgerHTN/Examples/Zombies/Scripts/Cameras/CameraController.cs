using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Cameras
{
    public class CameraController : MonoBehaviour
    {
        public CameraMovement cameraMovement;

        public void Update()
        {
            cameraMovement?.Update();
        }
    }
}