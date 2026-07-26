namespace BiomeWar
{
    /// <summary>Pure function: collectables found to star rating. Unit tested.</summary>
    public static class StarCalculator
    {
        public static int Calculate(int found, int total)
        {
            if (total <= 0) return 0;
            if (found <= 0) return 0;
            if (found >= total) return 3;

            float ratio = (float)found / total;
            if (ratio >= 2f / 3f) return 2;
            return 1;
        }
    }
}
