using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    public class HUDController : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] Image healthFill;

        [Header("Objectives")]
        [SerializeField] TextMeshProUGUI enemiesText;
        [SerializeField] TextMeshProUGUI relicsText;
        [SerializeField] TextMeshProUGUI scoreText;

        [Header("Countdown")]
        [SerializeField] GameObject countdownPanel;
        [SerializeField] TextMeshProUGUI countdownText;
        [SerializeField] float countdownShowAt = 3f;

        [Header("Abilities")]
        [SerializeField] Image ability1Fill;
        [SerializeField] Image ability2Fill;

        [Header("Boss")]
        [SerializeField] GameObject bossPanel;
        [SerializeField] Image bossFill;
        [SerializeField] TextMeshProUGUI bossName;

        [Header("Root")]
        [SerializeField] GameObject hudRoot;

        AbilityHolder abilities;

        void OnEnable()
        {
            GameEvents.OnPlayerHealthChanged += OnHealth;
            GameEvents.OnObjectiveUpdated += OnObjective;
            GameEvents.OnCollectablesUpdated += OnCollectables;
            GameEvents.OnScoreChanged += OnScore;
            GameEvents.OnPrepTimeTick += OnCountdownTick;
            GameEvents.OnPrepTimeEnded += OnCountdownEnded;
            GameEvents.OnBossSpawned += OnBossSpawned;
            GameEvents.OnBossHealthChanged += OnBossHealth;
            GameEvents.OnGameStateChanged += OnStateChanged;
        }

        void OnDisable()
        {
            GameEvents.OnPlayerHealthChanged -= OnHealth;
            GameEvents.OnObjectiveUpdated -= OnObjective;
            GameEvents.OnCollectablesUpdated -= OnCollectables;
            GameEvents.OnScoreChanged -= OnScore;
            GameEvents.OnPrepTimeTick -= OnCountdownTick;
            GameEvents.OnPrepTimeEnded -= OnCountdownEnded;
            GameEvents.OnBossSpawned -= OnBossSpawned;
            GameEvents.OnBossHealthChanged -= OnBossHealth;
            GameEvents.OnGameStateChanged -= OnStateChanged;
        }

        void Start()
        {
            if (bossPanel != null) bossPanel.SetActive(false);
            if (countdownPanel != null) countdownPanel.SetActive(false);

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) abilities = player.GetComponent<AbilityHolder>();
        }

        void Update()
        {
            if (abilities == null) return;

            UpdateCooldown(ability1Fill, 0);
            UpdateCooldown(ability2Fill, 1);
        }

        void UpdateCooldown(Image fill, int index)
        {
            if (fill == null || abilities.Abilities.Count <= index) return;

            var a = abilities.Abilities[index];
            fill.fillAmount = a.Cooldown <= 0f ? 0f : a.CooldownRemaining / a.Cooldown;
        }

        void OnHealth(float current, float max)
        {
            {
                Debug.Log($"HUD OnHealth: {current}/{max}, healthFill is {(healthFill == null ? "NULL" : "assigned")}");
                if (healthFill != null)
                    healthFill.fillAmount = max <= 0f ? 0f : current / max;
            }
            if (healthFill != null)
                healthFill.fillAmount = max <= 0f ? 0f : current / max;
        }

        void OnObjective(int remaining, int total)
        {
            if (enemiesText != null) enemiesText.text = $"{remaining} / {total}";
        }

        void OnCollectables(int found, int total)
        {
            if (relicsText != null) relicsText.text = $"{found} / {total}";
        }

        void OnScore(int score)
        {
            if (scoreText != null) scoreText.text = score.ToString();
        }

        // Only shows for the final few seconds so the exploration phase stays uncluttered.
        void OnCountdownTick(float remaining)
        {
            bool show = remaining <= countdownShowAt && remaining > 0f;

            if (countdownPanel != null && countdownPanel.activeSelf != show)
                countdownPanel.SetActive(show);

            if (show && countdownText != null)
                countdownText.text = Mathf.CeilToInt(remaining).ToString();
        }

        void OnCountdownEnded()
        {
            if (countdownPanel != null) countdownPanel.SetActive(false);
        }

        void OnBossSpawned(GameObject boss)
        {
            if (bossPanel != null) bossPanel.SetActive(true);

            var controller = boss.GetComponent<EnemyController>();
            if (bossName != null && controller != null && controller.Config != null)
                bossName.text = controller.Config.EnemyName;
        }

        void OnBossHealth(float current, float max)
        {
            if (bossFill != null)
                bossFill.fillAmount = max <= 0f ? 0f : current / max;

            if (current <= 0f && bossPanel != null)
                bossPanel.SetActive(false);
        }

        void OnStateChanged(GameStateId id)
        {
            if (hudRoot != null) hudRoot.SetActive(id == GameStateId.Playing);
        }
    }
}
