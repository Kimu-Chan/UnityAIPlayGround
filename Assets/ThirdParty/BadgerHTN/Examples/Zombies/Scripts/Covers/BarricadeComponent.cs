using BadgerHTN.Examples.Zombies.Scripts.Humanoids;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Covers
{
    public class BarricadeComponent : MonoBehaviour
    {
        public void Destroy()
        {
            GetComponent<DeathAnimation>()?.Activate();
        }
    }
}