using System.IO;
using UnityEngine;

namespace BiomeWar
{
    /// <summary>JSON persistence for progression, stats and settings.</summary>
    public class SaveManager : ManagerBase<SaveManager>
    {
        [SerializeField] private int totalLevels = 5;

        private const string FileName = "biomewar_save.json";
        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public SaveData Data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            Load();
        }

        public void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    Data = JsonUtility.FromJson<SaveData>(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] Load failed, creating new save. {e.Message}");
                Data = null;
            }

            if (Data == null) Data = new SaveData();
            EnsureLevelEntries();
        }

        public void Save()
        {
            try
            {
                string json = JsonUtility.ToJson(Data, true);
                File.WriteAllText(FilePath, json);

#if UNITY_WEBGL && !UNITY_EDITOR
                // WebGL writes to an in-memory filesystem; flush it to IndexedDB.
                WebGLFileSync.Sync();
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] Save failed. {e.Message}");
            }
        }

        private void EnsureLevelEntries()
        {
            for (int i = 0; i < totalLevels; i++)
            {
                if (Data.GetLevel(i) == null)
                {
                    Data.Levels.Add(new LevelProgressEntry
                    {
                        LevelIndex = i,
                        Unlocked = (i == 0),
                        Completed = false,
                        Stars = 0
                    });
                }
            }
        }

        public bool IsUnlocked(int index)
        {
            var e = Data.GetLevel(index);
            return e != null && e.Unlocked;
        }

        public int GetStars(int index)
        {
            var e = Data.GetLevel(index);
            return e == null ? 0 : e.Stars;
        }

        public void RecordLevelResult(LevelResult result)
        {
            var entry = Data.GetLevel(result.LevelIndex);
            if (entry == null) return;

            entry.Completed = true;
            if (result.Stars > entry.Stars) entry.Stars = result.Stars;
            if (result.Score > entry.BestScore) entry.BestScore = result.Score;
            if (entry.BestTimeSeconds <= 0f || result.TimeSeconds < entry.BestTimeSeconds)
                entry.BestTimeSeconds = result.TimeSeconds;

            Data.Stats.TotalKills += result.EnemiesDefeated;
            Data.Stats.TotalScore += result.Score;
            Data.Stats.CollectablesFound += result.CollectablesFound;

            int next = result.LevelIndex + 1;
            var nextEntry = Data.GetLevel(next);
            if (nextEntry != null && !nextEntry.Unlocked)
            {
                nextEntry.Unlocked = true;
                GameEvents.RaiseLevelUnlocked(next);
            }

            Save();
        }

        public void RecordDeath()
        {
            Data.Stats.TotalDeaths++;
            Save();
        }

        public void DeleteSave()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            Data = new SaveData();
            EnsureLevelEntries();
        }
    }
}
