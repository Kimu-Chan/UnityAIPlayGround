using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject
{
    [TaskDescription("Evade the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("cecc9277e75f9964e98d167be763695c", "992feefbe2d39f945b808bed5b4f0986")]
    public class Evade : IAstarAIMovement
    {
        [Tooltip("The agent has evaded when the magnitude is greater than this value")]
        public SharedFloat evadeDistance = 10;
        [Tooltip("The distance to look ahead when fleeing")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The transform that the agent is evading")]
        public SharedGameObject target;
        [Tooltip("The maximum number of interations that the position should be set")]
        public int maxInterations = 1;

        // The position of the target at the last frame
        private Vector3 targetPosition;

        public override void OnStart()
        {
            base.OnStart();

            targetPosition = target.Value.transform.position;
            if (maxInterations == 0) {
                Debug.LogWarning("Error: Max iterations must be greater than 0");
                maxInterations = 1;
            }
            SetDestination(Target(0));
        }

        // Evade from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            if (Vector3.Magnitude(transform.position - target.Value.transform.position) > evadeDistance.Value) {
                return TaskStatus.Success;
            }

            var interation = 0;
            while (!SetDestination(Target(interation)) && interation < maxInterations - 1) {
                interation++;
            }
            return TaskStatus.Running;
        }

        // Evade in the opposite direction
        private Vector3 Target(int iteration)
        {
            // Calculate the current distance to the target and the current speed
            var distance = (target.Value.transform.position - transform.position).magnitude;
            var speed = Velocity().magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value) {
                futurePrediction = targetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = target.Value.transform.position;
            var position = targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;

            return transform.position + (transform.position - position).normalized * lookAheadDistance.Value * ((maxInterations - iteration) / maxInterations);
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            target = null;
            evadeDistance = 10;
            lookAheadDistance = 5;
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
        }
    }
}