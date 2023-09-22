namespace BadgerHTN
{
    public enum Faction
    {
        None,
        Player,
        Enemy,
        Ally
    }

    public static class FactionExtensions
    {
        public static bool IsHostile(this Faction faction, Faction other)
        {
            return faction switch
            {
                Faction.Ally => other is Faction.Enemy,
                Faction.Enemy => other is Faction.Ally or Faction.Player,
                Faction.Player => other is Faction.Enemy,
                _ => false
            };
        }
    }
}