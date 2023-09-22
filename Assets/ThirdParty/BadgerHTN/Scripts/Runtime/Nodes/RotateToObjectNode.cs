using UnityEngine;

namespace BadgerHTN
{
    [NodeMenu("RotateToObject", "Navigation")]
    public class RotateToObjectNode : CustomNode<BaseContext>
    {
        private GameObject _target;
        [Blackboard(typeof(GameObject))] public string gameObject;

        protected override State OnExecute(BaseContext context)
        {
            if (context == null || string.IsNullOrEmpty(gameObject))
            {
                return State.Failed;
            }

            // Set target
            _target = context.Blackboard.Get<GameObject>(gameObject);
            if (_target == null)
            {
                return State.Failed;
            }

            var rotation = context.GameObject.GetComponent<IRotateTo>();
            if (rotation == null)
            {
                return State.Failed;
            }

            rotation.RotateTo(_target);

            if (rotation.IsDone())
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}