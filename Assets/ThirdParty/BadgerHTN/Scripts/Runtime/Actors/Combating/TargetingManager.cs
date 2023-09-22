using UnityEngine;

namespace BadgerHTN
{
    public class TargetingManager : MonoBehaviour
    {
        private const int SkipNumberOfFrames = 10;
        private int _frameCounter = 0;
        public static TargetingManager Instance { get; set; }

        protected virtual void FindTarget(Actor actor, Actor[] allActors)
        {
            var position = actor.Position;

            float minDistance = float.MaxValue;
            Actor bestTarget = null;

            foreach (var target in allActors)
            {
                if (actor == target)
                {
                    // Ignore self
                    continue;
                }

                if (target.health.IsDead)
                {
                    // Don't target dead actors
                    continue;
                }

                if (!actor.faction.IsHostile(target.faction))
                {
                    // Only target hostile actors
                    continue;
                }

                var distance = Vector3.Distance(position, target.Position);
                if (actor.combat.threatRange > 0 && distance > actor.combat.threatRange)
                {
                    // Skip targets outside threat range
                    continue;
                }

                if (distance < minDistance)
                {
                    // Update best target
                    bestTarget = target;
                    minDistance = distance;
                }
            }

            if (bestTarget != null)
            {
                // Set target to the closest one
                SetTarget(actor, bestTarget);
                UpdateTarget(actor, bestTarget);
            }
        }

        protected virtual Actor[] GetActors()
        {
            return FindObjectsOfType<Actor>();
        }

        protected virtual Actor GetTarget(Actor actor)
        {
            return actor?.Agent?.Blackboard?.Get<GameObject>("Target")?.GetComponent<Actor>();
        }

        protected virtual void SetTarget(Actor actor, Actor target)
        {
            bool hasTarget = target?.gameObject != null;
            actor?.Agent?.Blackboard?.Set<bool>("HasTarget", hasTarget);
            actor?.Agent?.Blackboard?.Set<GameObject>("Target", target?.gameObject);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_frameCounter++ % SkipNumberOfFrames != 0)
            {
                // Don't update every frame since FindObjectsOfType is very slow
                return;
            }

            var allActors = GetActors();
            foreach (var actor in allActors)
            {
                if (actor.health.IsDead)
                {
                    // Don't update
                    continue;
                }

                // Check if target is still alive if we already have a target
                var target = GetTarget(actor);
                if (target != null)
                {
                    UpdateTarget(actor, target);
                }

                // Find a new target if actor doesn't have one
                target = GetTarget(actor);
                if (target == null)
                {
                    FindTarget(actor, allActors);
                }
            }
        }

        private void UpdateTarget(Actor actor, Actor target)
        {
            // Remove target if dead
            if (target.health.IsDead)
            {
                SetTarget(actor, null);
            }

            if (actor.combat.attackRange > 0)
            {
                // Set target near if within attack range
                float distance = Vector3.Distance(actor.Position, target.Position);
                bool isTargetNear = distance <= actor.combat.attackRange;
                actor.Agent?.Blackboard?.Set<bool>("IsTargetNear", isTargetNear);
            }
        }
    }
}