using UnityEngine;

namespace BiomeWar
{
    public class GameManager : ManagerBase<GameManager>
    {
        private readonly StateMachine _states = new StateMachine();

        public MainMenuState MainMenu { get; private set; }
        public LevelSelectState LevelSelect { get; private set; }
        public BriefingState Briefing { get; private set; }
        public PlayingState Playing { get; private set; }
        public PausedState Paused { get; private set; }
        public LevelCompleteState LevelComplete { get; private set; }
        public GameOverState GameOver { get; private set; }

        public GameStateId CurrentStateId =>
            (_states.CurrentState as GameStateBase)?.Id ?? GameStateId.Boot;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            MainMenu = new MainMenuState(this);
            LevelSelect = new LevelSelectState(this);
            Briefing = new BriefingState(this);
            Playing = new PlayingState(this);
            Paused = new PausedState(this);
            LevelComplete = new LevelCompleteState(this);
            GameOver = new GameOverState(this);

            _states.ChangeState(Playing);
        }

        private void Update() => _states.Tick(Time.unscaledDeltaTime);

        public void ChangeState(IState next) => _states.ChangeState(next);

        public void SetCursorVisible(bool visible)
        {
#if UNITY_ANDROID || UNITY_IOS
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
#else
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
#endif
        }
    }
}
