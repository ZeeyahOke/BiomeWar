namespace BiomeWar
{
    public interface IEnemyBehaviour
    {
        void Initialise(EnemyContext context);
        void Tick(float deltaTime);
        void Exit();
    }
}
