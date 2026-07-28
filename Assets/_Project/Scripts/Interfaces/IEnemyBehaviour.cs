namespace BiomeWar
{
    /// <summary>
    /// Strategy pattern contract for enemy AI.
    /// Chase, Ranged and Defensive behaviours are interchangeable at runtime,
    /// which removes any need for conditional logic on enemy type.
    /// </summary>
    public interface IEnemyBehaviour
    {
        void Initialise(EnemyContext context);
        void Tick(float deltaTime);
        void Attack();
        void Exit();
    }
}
