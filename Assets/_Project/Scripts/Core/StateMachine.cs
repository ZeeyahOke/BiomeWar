namespace BiomeWar
{
    /// <summary>Holds one active state and delegates to it. No conditionals.</summary>
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void ChangeState(IState next)
        {
            if (next == null || next == CurrentState) return;
            CurrentState?.Exit();
            CurrentState = next;
            CurrentState.Enter();
        }

        public void Tick(float deltaTime)
        {
            CurrentState?.Tick(deltaTime);
        }

        public void Clear()
        {
            CurrentState?.Exit();
            CurrentState = null;
        }
    }
}
