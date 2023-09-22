using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement movement;
        public PlayerRotation rotation;

        public void Update()
        {
            movement?.Update();
            rotation?.Update();
        }
    }
}