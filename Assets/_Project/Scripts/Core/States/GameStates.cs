using UnityEngine;

namespace BiomeWar
{
    public abstract class GameStateBase : IState
    {
        protected readonly GameManager Game;
        public abstract GameStateId Id { get; }

        protected GameStateBase(GameManager game) { Game = game; }

        public virtual void Enter()
        {
            GameEvents.RaiseGameStateChanged(Id);
        }

        public virtual void Tick(float deltaTime) { }
        public virtual void Exit() { }
    }

    public class MainMenuState : GameStateBase
    {
        public override GameStateId Id => GameStateId.MainMenu;
        public MainMenuState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 1f;
            Game.SetCursorVisible(true);
        }
    }

    public class LevelSelectState : GameStateBase
    {
        public override GameStateId Id => GameStateId.LevelSelect;
        public LevelSelectState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 1f;
            Game.SetCursorVisible(true);
        }
    }

    public class BriefingState : GameStateBase
    {
        public override GameStateId Id => GameStateId.Briefing;
        public BriefingState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Game.SetCursorVisible(true);
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }

    public class PlayingState : GameStateBase
    {
        public override GameStateId Id => GameStateId.Playing;
        public PlayingState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 1f;
            Game.SetCursorVisible(false);
        }
    }

    public class PausedState : GameStateBase
    {
        public override GameStateId Id => GameStateId.Paused;
        public PausedState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Game.SetCursorVisible(true);
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }

    public class LevelCompleteState : GameStateBase
    {
        public override GameStateId Id => GameStateId.LevelComplete;
        public LevelCompleteState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Game.SetCursorVisible(true);
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }

    public class GameOverState : GameStateBase
    {
        public override GameStateId Id => GameStateId.GameOver;
        public GameOverState(GameManager g) : base(g) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Game.SetCursorVisible(true);
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}
