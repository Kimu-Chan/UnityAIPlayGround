using BadgerHTN.Examples.Zombies.Scripts.Guns;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Nodes
{
    [NodeMenu("Shoot", "Combat")]
    public class ShootNode : CustomNode<BaseContext>
    {
        [Blackboard(typeof(GameObject))] public string gameObject;
        private GameObject _target;

        protected override State OnExecute(BaseContext context)
        {
            var gun = context.GameObject.GetComponentInChildren<IGun>();
            if (gun == null)
            {
                return State.Failed;
            }

            if (string.IsNullOrEmpty(gameObject))
            {
                return State.Failed;
            }

            _target = context.Agent.Blackboard.Get<GameObject>("Target");
            if (_target == null)
            {
                return State.Failed;
            }

            if (gun.Fire())
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}