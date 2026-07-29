using System;
using UnityEngine;

namespace BiomeWar
{
    /// <summary>Observer pattern event bus. Decouples gameplay, UI, audio and VFX.</summary>
    public static class GameEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetOnLoad()
        {
            ClearAll();
        }
        // Player
        public static event Action<float, float> OnPlayerHealthChanged;
        public static event Action<DamageInfo> OnPlayerDamaged;
        public static event Action OnPlayerDied;
        public static event Action<IAbility> OnAbilityActivated;

        // Enemies
        public static event Action<GameObject> OnEnemyDefeated;
        public static event Action<float, float> OnBossHealthChanged;
        public static event Action<GameObject> OnBossSpawned;

        // Objectives
        public static event Action<int, int> OnObjectiveUpdated;
        public static event Action<string> OnItemCollected;
        public static event Action<int, int> OnCollectablesUpdated;

        // Level flow
        public static event Action<int> OnLevelStarted;
        public static event Action<LevelResult> OnLevelCompleted;
        public static event Action<int> OnLevelUnlocked;

        // Global
        public static event Action<GameStateId> OnGameStateChanged;
        public static event Action<int> OnScoreChanged;

        public static event Action<float> OnPrepTimeTick;
        public static event Action OnPrepTimeEnded;

        public static void RaisePlayerHealthChanged(float current, float max)
            => OnPlayerHealthChanged?.Invoke(current, max);

        public static void RaisePlayerDamaged(DamageInfo info)
            => OnPlayerDamaged?.Invoke(info);

        public static void RaisePlayerDied()
            => OnPlayerDied?.Invoke();

        public static void RaiseAbilityActivated(IAbility ability)
            => OnAbilityActivated?.Invoke(ability);

        public static void RaiseEnemyDefeated(GameObject enemy)
            => OnEnemyDefeated?.Invoke(enemy);

        public static void RaiseBossHealthChanged(float current, float max)
            => OnBossHealthChanged?.Invoke(current, max);

        public static void RaiseBossSpawned(GameObject boss)
            => OnBossSpawned?.Invoke(boss);

        public static void RaiseObjectiveUpdated(int remaining, int total)
            => OnObjectiveUpdated?.Invoke(remaining, total);

        public static void RaiseItemCollected(string id)
            => OnItemCollected?.Invoke(id);

        public static void RaiseCollectablesUpdated(int found, int total)
            => OnCollectablesUpdated?.Invoke(found, total);

        public static void RaiseLevelStarted(int levelIndex)
            => OnLevelStarted?.Invoke(levelIndex);

        public static void RaiseLevelCompleted(LevelResult result)
            => OnLevelCompleted?.Invoke(result);

        public static void RaiseLevelUnlocked(int levelIndex)
            => OnLevelUnlocked?.Invoke(levelIndex);

        public static void RaiseGameStateChanged(GameStateId state)
            => OnGameStateChanged?.Invoke(state);

        public static void RaiseScoreChanged(int score)
            => OnScoreChanged?.Invoke(score);

        public static void RaisePrepTimeTick(float remaining)
        => OnPrepTimeTick?.Invoke(remaining);

        public static void RaisePrepTimeEnded()
        => OnPrepTimeEnded?.Invoke();

        public static void ClearAll()
        {
            OnPlayerHealthChanged = null;
            OnPlayerDamaged = null;
            OnPlayerDied = null;
            OnAbilityActivated = null;
            OnEnemyDefeated = null;
            OnBossHealthChanged = null;
            OnBossSpawned = null;
            OnObjectiveUpdated = null;
            OnItemCollected = null;
            OnCollectablesUpdated = null;
            OnLevelStarted = null;
            OnLevelCompleted = null;
            OnLevelUnlocked = null;
            OnGameStateChanged = null;
            OnScoreChanged = null;
            OnPrepTimeTick = null;
            OnPrepTimeEnded = null;
        }
    }
}
