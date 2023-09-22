using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Guns
{
    public class GunComponent : MonoBehaviour, IGun
    {
        public float bulletForce;
        public Transform bulletPoint;
        public float bulletSpeed;
        public int damage;
        public GameObject prefab;
        public Timer timerFire;
        private BoxCollider _collider;

        public bool Fire()
        {
            if (timerFire.HasElapsed(Time.time))
            {
                SpawnBullet();
                timerFire.Set(Time.time);
                return true;
            }

            return false;
        }

        public void Ragdoll()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }

            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void SpawnBullet()
        {
            var t = bulletPoint.transform;
            var obj = Instantiate(prefab, t.position, t.rotation);
            var bullet = obj.GetComponent<BulletComponent>().bullet;

            bullet.damage = damage;
            bullet.force = bulletForce;
            bullet.SetVelocity(bulletPoint.forward * bulletSpeed);

            var actor = GetComponentInParent<Actor>();
            if (actor != null)
            {
                bullet.faction = actor.faction;
            }
        }
    }
}