using UnityEngine;

namespace BadgerHTN.Examples.Zombies.Scripts.Guns
{
    public class BulletComponent : MonoBehaviour
    {
        public Bullet bullet;

        public void Update()
        {
            if (bullet.HasExpired())
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var ownerFaction = bullet.faction;
            var actorOther = collision?.gameObject?.GetComponentInParent<Actor>();
            if (actorOther != null)
            {
                if (ownerFaction != Faction.None && ownerFaction != actorOther.faction)
                {
                    actorOther.ChangeHealth(-bullet.damage);
                }
            }

            Destroy(gameObject);
        }
    }
}