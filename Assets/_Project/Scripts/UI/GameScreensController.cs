using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    public class GameScreensController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] GameObject briefingPanel;
        [SerializeField] GameObject pausePanel;
        [SerializeField] GameObject completePanel;
        [SerializeField] GameObject gameOverPanel;

        [Header("Briefing")]
        [SerializeField] TextMeshProUGUI briefingTitle;
        [SerializeField] TextMeshProUGUI briefingDescription;
        [SerializeField] TextMeshProUGUI briefingObjective;
        [SerializeField] TextMeshProUGUI briefingControls;

        [Header("Level complete")]
        [SerializeField] TextMeshProUGUI completeScore;
        [SerializeField] TextMeshProUGUI completeTime;
        [SerializeField] TextMeshProUGUI completeRelics;
        [SerializeField] Image[] starImages;
        [SerializeField] Sprite starFilled;
        [SerializeField] Sprite starEmpty;
        [SerializeField] Button nextLevelButton;

        [Header("Game over")]
        [SerializeField] TextMeshProUGUI gameOverScore;

        [SerializeField] ObjectiveTracker tracker;

        void OnEnable()
        {
            GameEvents.OnGameStateChanged += OnStateChanged;
            GameEvents.OnLevelCompleted += OnLevelCompleted;
        }

        void OnDisable()
        {
            GameEvents.OnGameStateChanged -= OnStateChanged;
            GameEvents.OnLevelCompleted -= OnLevelCompleted;
        }

        void Start()
        {
            HideAll();
            ShowBriefing();
        }

        void Update()
        {
            if (!InputReader.Exists || !GameManager.Exists) return;
            if (!InputReader.Instance.PausePressed) return;

            var gm = GameManager.Instance;

            if (gm.CurrentStateId == GameStateId.Playing) gm.ChangeState(gm.Paused);
            else if (gm.CurrentStateId == GameStateId.Paused) gm.ChangeState(gm.Playing);
        }

        void HideAll()
        {
            if (briefingPanel != null) briefingPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (completePanel != null) completePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        void ShowBriefing()
        {
            var config = tracker != null ? tracker.Config : null;
            if (config == null) return;

            if (briefingTitle != null) briefingTitle.text = config.LevelName;
            if (briefingDescription != null) briefingDescription.text = config.BiomeDescription;
            if (briefingObjective != null) briefingObjective.text = config.ObjectiveText;

            if (briefingControls != null)
            {
#if UNITY_ANDROID || UNITY_IOS
                briefingControls.text = "Use the d-pad to move. Tap FIRE to shoot. Tap the ability icons to use Slam and Dash.";
#else
                briefingControls.text = "WASD or arrow keys to move. Left click to fire. E to interact. Q for Slam, F for Dash. Esc to pause.";
#endif
            }

            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.Briefing);
        }

        void OnStateChanged(GameStateId id)
        {
            if (briefingPanel != null) briefingPanel.SetActive(id == GameStateId.Briefing);
            if (pausePanel != null) pausePanel.SetActive(id == GameStateId.Paused);
            if (completePanel != null) completePanel.SetActive(id == GameStateId.LevelComplete);
            if (gameOverPanel != null) gameOverPanel.SetActive(id == GameStateId.GameOver);
        }

        void OnLevelCompleted(LevelResult result)
        {
            if (completeScore != null) completeScore.text = result.Score.ToString();

            if (completeTime != null)
            {
                int minutes = Mathf.FloorToInt(result.TimeSeconds / 60f);
                int seconds = Mathf.FloorToInt(result.TimeSeconds % 60f);
                completeTime.text = $"{minutes:00}:{seconds:00}";
            }

            if (completeRelics != null)
                completeRelics.text = $"{result.CollectablesFound} / {result.CollectablesTotal}";

            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] == null) continue;
                starImages[i].sprite = i < result.Stars ? starFilled : starEmpty;
            }

            if (nextLevelButton != null)
            {
                bool hasNext = LevelManager.Exists &&
                               LevelManager.Instance.GetLevel(result.LevelIndex + 1) != null;
                nextLevelButton.gameObject.SetActive(hasNext);
            }
        }

        // ---- Button hooks ----

        public void OnStartLevel()
        {
            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.Playing);
        }

        public void OnResume()
        {
            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.Playing);
        }

        public void OnRetry()
        {
            if (LevelManager.Exists) LevelManager.Instance.ReloadCurrentLevel();
        }

        public void OnNextLevel()
        {
            if (LevelManager.Exists) LevelManager.Instance.LoadNextLevel();
        }

        public void OnMainMenu()
        {
            if (LevelManager.Exists) LevelManager.Instance.LoadMainMenu();
        }

        public void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
