using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class NpcController : MonoBehaviour
    {
        public NpcRotation rotation;

        public void Update()
        {
            rotation?.Update();
        }
    }
}