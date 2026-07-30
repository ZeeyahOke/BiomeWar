using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] GameObject mainPanel;
        [SerializeField] GameObject levelSelectPanel;
        [SerializeField] GameObject settingsPanel;

        [Header("Buttons")]
        [SerializeField] Button continueButton;

        void Start()
        {
            ShowMain();

            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.MainMenu);

            // Continue is only useful if at least one level has been completed.
            if (continueButton != null)
                continueButton.gameObject.SetActive(HasProgress());
        }

        bool HasProgress()
        {
            if (!SaveManager.Exists) return false;

            foreach (var level in SaveManager.Instance.Data.Levels)
                if (level.Completed) return true;

            return false;
        }

        public void ShowMain()
        {
            SetPanels(true, false, false);
        }

        public void ShowLevelSelect()
        {
            SetPanels(false, true, false);
        }

        public void ShowSettings()
        {
            SetPanels(false, false, true);
        }

        void SetPanels(bool main, bool levels, bool settings)
        {
            if (mainPanel != null) mainPanel.SetActive(main);
            if (levelSelectPanel != null) levelSelectPanel.SetActive(levels);
            if (settingsPanel != null) settingsPanel.SetActive(settings);
        }

        public void OnStartGame()
        {
            if (LevelManager.Exists) LevelManager.Instance.LoadLevel(0);
        }

        // Loads the highest unlocked level.
        public void OnContinue()
        {
            if (!SaveManager.Exists || !LevelManager.Exists) return;

            int target = 0;
            foreach (var level in SaveManager.Instance.Data.Levels)
                if (level.Unlocked && level.LevelIndex > target) target = level.LevelIndex;

            LevelManager.Instance.LoadLevel(target);
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
