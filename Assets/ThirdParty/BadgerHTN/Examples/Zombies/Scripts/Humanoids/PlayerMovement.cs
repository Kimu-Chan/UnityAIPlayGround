using System;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    [Serializable]
    public class PlayerMovement
    {
        public CharacterController controller;
        public float speed;

        public void Update()
        {
            var dir = DirectionHelper.GetInputDirection();

            var velocity = dir * speed;
            var motion = velocity * Time.deltaTime;

            if (controller != null)
            {
                controller.Move(motion);
            }
        }
    }
}