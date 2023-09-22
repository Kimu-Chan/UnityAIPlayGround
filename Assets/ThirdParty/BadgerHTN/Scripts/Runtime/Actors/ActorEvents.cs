namespace BadgerHTN
{
    public static class ActorEvents
    {
        public static HealthHandler HealthChanged { get; set; }

        public delegate void HealthHandler(Actor actor, CombatInfo info = default);
    }
}