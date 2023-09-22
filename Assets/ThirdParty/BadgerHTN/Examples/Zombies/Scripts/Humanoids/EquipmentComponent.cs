using BadgerHTN.Examples.Zombies.Scripts.Guns;
using BadgerHTN.Examples.Zombies.Scripts.Pickups;
using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Humanoids
{
    public class EquipmentComponent : MonoBehaviour
    {
        public bool isPlayerControlled;

        public KeyCode[] keysFire =
        {
            KeyCode.Space,
            KeyCode.Mouse0
        };

        public Transform slot;
        public GameObject startItem;
        private GameObject _item;

        private void OnTriggerEnter(Collider other)
        {
            var pickup = other.GetComponentInParent<PickupComponent>();
            if (pickup != null)
            {
                var tmp = _item;

                var obj = pickup.Activate();
                Set(obj);

                if (obj != null && tmp != null)
                {
                    pickup.Set(tmp);
                }
            }
        }

        private void Set(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            _item = obj;
            var t = obj.transform;
            t.parent = slot;
            t.localRotation = Quaternion.identity;

            var pos = Vector3.zero;

            var anchor = obj.GetComponentInChildren<GunAnchor>();
            if (anchor != null)
            {
                pos -= anchor.transform.localPosition;
            }

            t.localPosition = pos;
        }

        private void Start()
        {
            if (startItem != null)
            {
                var obj = Instantiate(startItem);
                Set(obj);
            }
        }

        private void Update()
        {
            if (_item == null)
            {
                return;
            }

            if (isPlayerControlled)
            {
                foreach (var key in keysFire)
                {
                    if (Input.GetKey(key))
                    {
                        var gunComponent = _item?.GetComponent<GunComponent>();
                        if (gunComponent != null)
                        {
                            gunComponent.Fire();
                        }
                    }
                }
            }
        }
    }
}