namespace BiomeWar
{
    /// <summary>Pure function: collectables found to star rating. Unit tested.</summary>
    public static class StarCalculator
    {
        public static int Calculate(int found, int total)
        {
            if (total <= 0 || found <= 0) return 0;
            return found > 3 ? 3 : found;
        }
    }
}
