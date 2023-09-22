using Task = System.Threading.Tasks.Task;

namespace BadgerHTN.Examples.Zombies.Scripts.Ui
{
    public class HealthBar : Progress
    {
        private Actor _actor;

        private void Awake()
        {
            _actor = GetComponentInParent<Actor>(true);
            ActorEvents.HealthChanged += OnHealthChanged;
        }

        private async void HideBar()
        {
            await Task.Delay(1000);

            if (this != null && gameObject != null)
            {
                gameObject?.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            ActorEvents.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(Actor actor, CombatInfo info)
        {
            if (actor != _actor)
            {
                return;
            }

            var health = actor.health;
            var progress = health.hitPoints / (float)health.maxHitPoints;
            SetFill(progress);

            if (health.hitPoints <= 0)
            {
                HideBar();
            }
        }
    }
}