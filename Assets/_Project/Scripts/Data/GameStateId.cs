namespace BiomeWar
{
    /// <summary>
    /// Identifier only. State behaviour lives in polymorphic state classes —
    /// this is never switched on to decide what a state does.
    /// </summary>
    public enum GameStateId
    {
        Boot,
        MainMenu,
        LevelSelect,
        Briefing,
        Playing,
        Paused,
        LevelComplete,
        GameOver
    }
}
