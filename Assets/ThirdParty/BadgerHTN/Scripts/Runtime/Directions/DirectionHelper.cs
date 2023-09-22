using UnityEngine;

namespace BadgerHTN
{
    public static class DirectionHelper
    {
        public static Vector3 GetInputDirection()
        {
            var vertical = Input.GetAxisRaw("Vertical");
            var horizontal = Input.GetAxisRaw("Horizontal");

            // Flatten y-axis
            var direction = new Vector3(horizontal, 0, vertical);

            var i = direction;
            if (i.magnitude > 1)
            {
                // Normalize
                i /= i.magnitude;
            }

            return i;
        }

        public static Vector3 RectangularToEuler(Vector3 direction)
        {
            float y = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z);
            var eulerAngles = new Vector3(0, y, 0);
            return eulerAngles;
        }
    }
}