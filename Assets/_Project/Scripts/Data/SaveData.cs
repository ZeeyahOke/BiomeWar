using System;
using System.Collections.Generic;

namespace BiomeWar
{
    [Serializable]
    public class LevelProgressEntry
    {
        public int LevelIndex;
        public bool Unlocked;
        public bool Completed;
        public int Stars;
        public int BestScore;
        public float BestTimeSeconds;
    }

    [Serializable]
    public class PlayerStatsData
    {
        public int TotalKills;
        public int TotalDeaths;
        public int TotalScore;
        public int CollectablesFound;
    }

    [Serializable]
    public class SettingsData
    {
        public float MusicVolume = 0.7f;
        public float SfxVolume = 1f;
        public float MouseSensitivity = 2f;
        public int QualityLevel = 2;
    }

/// <summary>Root serialised object. JsonUtility supports Lists, not Dictionaries.</summary>
    [Serializable]
    public class SaveData
    {
        public int SaveVersion = 1;
        public List<LevelProgressEntry> Levels = new List<LevelProgressEntry>();
        public List<string> CollectedItemIds = new List<string>();
        public PlayerStatsData Stats = new PlayerStatsData();
        public SettingsData Settings = new SettingsData();

        public LevelProgressEntry GetLevel(int index)
        {
            return Levels.Find(l => l.LevelIndex == index);
        }
    }
}
