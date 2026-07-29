using UnityEngine;
using UnityEngine.SceneManagement;

namespace BiomeWar
{
    public class LevelManager : ManagerBase<LevelManager>
    {
        [SerializeField] LevelConfig[] levels;
        [SerializeField] string mainMenuScene = "MainMenu";

        public LevelConfig[] Levels => levels;
        public LevelConfig CurrentLevel { get; private set; }

        void OnEnable() => GameEvents.OnLevelCompleted += OnLevelCompleted;
        void OnDisable() => GameEvents.OnLevelCompleted -= OnLevelCompleted;

        public LevelConfig GetLevel(int index)
        {
            foreach (var l in levels)
                if (l != null && l.LevelIndex == index) return l;
            return null;
        }

        public bool IsUnlocked(int index)
        {
            return SaveManager.Exists && SaveManager.Instance.IsUnlocked(index);
        }

        public void LoadLevel(int index)
        {
            var level = GetLevel(index);
            if (level == null)
            {
                Debug.LogError($"No LevelConfig with index {index}.");
                return;
            }

            if (!IsUnlocked(index))
            {
                Debug.LogWarning($"Level {index} is locked.");
                return;
            }

            CurrentLevel = level;
            SceneManager.LoadScene(level.SceneName);
        }

        public void ReloadCurrentLevel()
        {
            if (CurrentLevel != null) SceneManager.LoadScene(CurrentLevel.SceneName);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            if (CurrentLevel == null) return;

            int next = CurrentLevel.LevelIndex + 1;
            if (GetLevel(next) == null || !IsUnlocked(next))
            {
                LoadMainMenu();
                return;
            }

            LoadLevel(next);
        }

        public void LoadMainMenu()
        {
            CurrentLevel = null;
            SceneManager.LoadScene(mainMenuScene);
        }

        void OnLevelCompleted(LevelResult result)
        {
            if (SaveManager.Exists)
                SaveManager.Instance.RecordLevelResult(result);

            if (GameManager.Exists)
                GameManager.Instance.ChangeState(GameManager.Instance.LevelComplete);
        }

        
    }
}
