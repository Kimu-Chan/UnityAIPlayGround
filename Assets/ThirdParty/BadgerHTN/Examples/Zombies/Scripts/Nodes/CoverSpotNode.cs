using BadgerHTN.Examples.Zombies.Scripts.Covers;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Nodes
{
    [NodeMenu("CoverSpot", "Examples")]
    public class CoverSpotNode : CustomNode<BaseContext>
    {
        private GameObject _player;

        protected override State OnExecute(BaseContext context)
        {
            var coverManager = CoverManager.Instance;
            if (coverManager == null)
            {
                return State.Failed;
            }

            var cover = coverManager.Reserve(context.Agent, context.Transform);

            bool hasCover = cover != null;
            context.Blackboard.Set<GameObject>("Cover", cover.gameObject);

            return hasCover
                ? State.Success
                : State.Failed;
        }

        protected override bool OnQuery(BaseContext context)
        {
            var coverManager = CoverManager.Instance;
            if (coverManager == null)
            {
                return false;
            }

            var hasAvailableCover = coverManager.HasAvailableCover(context.Agent);
            return hasAvailableCover;
        }
    }
}