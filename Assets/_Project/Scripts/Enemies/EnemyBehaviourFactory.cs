namespace BiomeWar
{
    // The only place behaviour type maps to a concrete class. Adding a behaviour
    // means one case here plus the new class - nothing else changes.
    public static class EnemyBehaviourFactory
    {
        public static IEnemyBehaviour Create(EnemyBehaviourType type)
        {
            switch (type)
            {
                case EnemyBehaviourType.Ranged:    return new RangedBehaviour();
                case EnemyBehaviourType.Defensive: return new DefensiveBehaviour();
                default:                           return new ChaseBehaviour();
            }
        }
    }
}
