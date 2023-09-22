using System;
using UnityEngine;

namespace BadgerHTN
{
    [Serializable]
    public class Combat
    {
        [Tooltip("A range of zero or lower will be ignored")]
        public float attackRange;

        public int damage;

        [Tooltip("A range of zero or lower will be ignored")]
        public float threatRange;
    }
}