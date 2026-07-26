using System;
using System.Collections.Generic;
using UnityEngine;

namespace BiomeWar
{
    [Serializable]
    public class EnemySpawnGroup
    {
        public EnemyConfig Config;
        public int Count = 5;
    }

    /// <summary>Data-driven level definition. Levels are data, not code.</summary>
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "BiomeWar/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Identity")]
        public int LevelIndex;
        public string LevelName = "Beach";
        public string SceneName = "Level_01_Beach";
        public Sprite Thumbnail;

        [Header("Briefing")]
        [TextArea(2, 4)] public string BiomeDescription;
        [TextArea(2, 4)] public string ObjectiveText;

        [Header("Enemies")]
        public List<EnemySpawnGroup> SpawnGroups = new List<EnemySpawnGroup>();

        [Header("Collectables")]
        public int CollectableCount = 3;

        [Header("Boss")]
        public bool HasBoss;
        public EnemyConfig BossConfig;

        [Header("Audio")]
        public AudioClip MusicTrack;

        public int TotalEnemies
        {
            get
            {
                int t = 0;
                foreach (var g in SpawnGroups) t += g.Count;
                if (HasBoss) t += 1;
                return t;
            }
        }
    }
}
