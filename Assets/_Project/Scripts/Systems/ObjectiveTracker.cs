using UnityEngine;

namespace BiomeWar
{
    public class ObjectiveTracker : MonoBehaviour
    {
        [SerializeField] LevelConfig config;

        int enemiesTotal;
        int enemiesDefeated;
        int collectablesTotal;
        int collectablesFound;
        int score;
        float elapsed;
        bool complete;

        public LevelConfig Config => config;
        public int EnemiesRemaining => enemiesTotal - enemiesDefeated;

        void OnEnable()
        {
            GameEvents.OnEnemyDefeated += OnEnemyDefeated;
            GameEvents.OnItemCollected += OnItemCollected;
        }

        void OnDisable()
        {
            GameEvents.OnEnemyDefeated -= OnEnemyDefeated;
            GameEvents.OnItemCollected -= OnItemCollected;
        }

        void Start()
        {
            if (config == null)
            {
                Debug.LogError("ObjectiveTracker has no LevelConfig.");
                enabled = false;
                return;
            }

            enemiesTotal = config.TotalEnemies;
            collectablesTotal = config.CollectableCount;

            GameEvents.RaiseLevelStarted(config.LevelIndex);
            GameEvents.RaiseObjectiveUpdated(enemiesTotal, enemiesTotal);
            GameEvents.RaiseCollectablesUpdated(0, collectablesTotal);
        }

        void Update()
        {
            if (complete) return;
            if (GameManager.Exists && GameManager.Instance.CurrentStateId != GameStateId.Playing) return;
            elapsed += Time.deltaTime;
        }

        void OnEnemyDefeated(GameObject enemy)
        {
            if (complete) return;

            enemiesDefeated++;

            var controller = enemy.GetComponent<EnemyController>();
            if (controller != null && controller.Config != null)
            {
                score += controller.Config.ScoreValue;
                GameEvents.RaiseScoreChanged(score);
            }

            GameEvents.RaiseObjectiveUpdated(EnemiesRemaining, enemiesTotal);

            if (EnemiesRemaining <= 0) CompleteLevel();
        }

        void OnItemCollected(string id)
        {
            if (complete) return;

            collectablesFound++;
            GameEvents.RaiseCollectablesUpdated(collectablesFound, collectablesTotal);
        }

        void CompleteLevel()
        {
            complete = true;

            int stars = StarCalculator.Calculate(collectablesFound, collectablesTotal);
            score += stars * 250;

            float scoreMult = DailyChallengeService.Exists
                ? DailyChallengeService.Instance.Active.scoreMultiplier
                : 1f;

            score = Mathf.RoundToInt(score * scoreMult);

            var result = new LevelResult(
                config.LevelIndex,
                enemiesDefeated,
                collectablesFound,
                collectablesTotal,
                elapsed,
                stars,
                score);

            GameEvents.RaiseLevelCompleted(result);
        }
    }
}
