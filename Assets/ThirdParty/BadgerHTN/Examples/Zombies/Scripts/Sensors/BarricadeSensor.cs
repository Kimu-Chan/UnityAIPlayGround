using BadgerHTN.Examples.Zombies.Scripts.Covers;
using BadgerHTN.Examples.Zombies.Scripts.Humanoids;

namespace BadgerHTN.Examples.Zombies.Scripts.Sensors
{
    public class BarricadeSensor : Sensor<BarricadeComponent>
    {
        protected override void OnAdd(BarricadeComponent component)
        {
            var deathComponent = component?.GetComponent<DeathAnimation>();
            if (deathComponent != null)
            {
                deathComponent.Activate();
            }
        }
    }
}