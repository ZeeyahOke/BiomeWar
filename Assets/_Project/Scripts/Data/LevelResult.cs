namespace BiomeWar
{
     /// <summary>Summary of a completed level.</summary>
    public struct LevelResult
    {
        public int LevelIndex;
        public int EnemiesDefeated;
        public int CollectablesFound;
        public int CollectablesTotal;
        public float TimeSeconds;
        public int Stars;
        public int Score;

        public LevelResult(int levelIndex, int enemiesDefeated, int collectablesFound,
                           int collectablesTotal, float timeSeconds, int stars, int score)
        {
            LevelIndex = levelIndex;
            EnemiesDefeated = enemiesDefeated;
            CollectablesFound = collectablesFound;
            CollectablesTotal = collectablesTotal;
            TimeSeconds = timeSeconds;
            Stars = stars;
            Score = score;
        }
    }
}
