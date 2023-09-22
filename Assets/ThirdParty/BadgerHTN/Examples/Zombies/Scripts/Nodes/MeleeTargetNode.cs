using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Nodes
{
    [NodeMenu("MeleeTarget", "Examples")]
    public class MeleeTargetNode : CustomNode<BaseContext>
    {
        [Expose] public float delay = 0.1f;
        private GameObject _player;

        private GameObject _target;
        private float _timestamp;

        protected override State OnExecute(BaseContext context)
        {
            _target = context.Blackboard.Get<GameObject>("Target");
            var actor = context.GameObject.GetComponent<Actor>();
            var targetActor = _target?.GetComponent<Actor>();
            if (_target == null || actor == null || targetActor == null)
            {
                return State.Failed;
            }

            if (_timestamp <= 0)
            {
                targetActor.ChangeHealth(-actor.combat.damage);
                _timestamp = Time.time;

                if (targetActor.health.IsDead)
                {
                    context.Blackboard.Set("HasTarget", false);
                    context.Blackboard.Set("IsTargetNear", false);
                }
            }

            // Wait for cooldown
            var hasDelayElapsed = Time.time > _timestamp + delay;

            // Return running if waiting for cooldown
            return hasDelayElapsed
                ? State.Success
                : State.Running;
        }
    }
}