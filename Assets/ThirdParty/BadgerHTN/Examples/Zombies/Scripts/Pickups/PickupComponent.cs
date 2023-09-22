using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Pickups
{
    public class PickupComponent : MonoBehaviour
    {
        public float amplitude = 0.5f;
        public float frequency = 1f;
        public GameObject item;
        public Transform itemTransform;
        public GameObject prefab;

        public float rotationSpeed = 10f;

        public GameObject Activate()
        {
            var tmp = item;

            item = null;
            return tmp;
        }

        public void Set(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            obj.transform.parent = itemTransform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            item = obj;
        }

        public void Spawn()
        {
            item = Instantiate(prefab, itemTransform);
            item.transform.localPosition = Vector3.zero;
        }

        private void Awake()
        {
            Spawn();
        }

        private void Update()
        {
            if (item != null)
            {
                var speed = rotationSpeed * Time.deltaTime;
                var t = item.transform;
                t.Rotate(new Vector3(0, speed, 0));

                var y = Mathf.Sin(Time.time * frequency) - 0.5f;

                var pos = t.localPosition;
                pos.y = y * amplitude;
                t.localPosition = pos;
            }
        }
    }
}