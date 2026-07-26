namespace BiomeWar
{
    /// <summary>State pattern contract. Behaviour lives in the state object itself.</summary>
    public interface IState
    {
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }
}
