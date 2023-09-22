using System;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Guns
{
    [Serializable]
    public class Bullet
    {
        public float aliveDuration;
        public int damage;
        public Faction faction;
        public float force;
        public Rigidbody rigidbody;
        private float _timestamp;

        public bool HasExpired()
        {
            return Time.time > aliveDuration + _timestamp;
        }

        public void SetVelocity(Vector3 velocity)
        {
            rigidbody.velocity = velocity;
            _timestamp = Time.time;
        }
    }
}