using UnityEngine;
using UnityEngine.Events;

namespace BiomeWar
{
    /// <summary>IDamageable implementation used by player, enemies and props.</summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private bool isPlayer;
        [SerializeField] private bool invulnerable;

        [Header("Directional defence (Defensive enemies)")]
        [SerializeField] private bool useDirectionalResistance;
        [SerializeField, Range(0f, 1f)] private float frontalReduction = 0.8f;
        [SerializeField] private float blockAngle = 90f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public bool IsAlive => CurrentHealth > 0f;

        public UnityEvent<float, float> OnHealthChangedLocal;
        public UnityEvent OnDiedLocal;

        public event System.Action<DamageInfo> OnDamaged;
        public event System.Action OnDied;

        private void Awake() => CurrentHealth = maxHealth;

        public void Configure(float newMax, bool directional = false, float reduction = 0f, float angle = 90f)
        {
            maxHealth = newMax;
            CurrentHealth = newMax;
            useDirectionalResistance = directional;
            frontalReduction = reduction;
            blockAngle = angle;
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            Broadcast();
        }

        public void TakeDamage(DamageInfo info)
        {
            if (!IsAlive || invulnerable) return;

            float amount = info.Amount;

            if (useDirectionalResistance && info.HitDirection != Vector3.zero)
            {
                float resist = DamageCalculator.DirectionalResistance(
                    transform.forward, info.HitDirection, blockAngle, frontalReduction);
                amount *= (1f - resist);
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);

            OnDamaged?.Invoke(info);
            Broadcast();

            if (isPlayer) GameEvents.RaisePlayerDamaged(info);

            if (!IsAlive) Die();
            Debug.Log($"{name} took {amount} damage. HP: {CurrentHealth}/{maxHealth}");
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
            Broadcast();
        }

        private void Broadcast()
        {
            OnHealthChangedLocal?.Invoke(CurrentHealth, maxHealth);
            if (isPlayer) GameEvents.RaisePlayerHealthChanged(CurrentHealth, maxHealth);
        }

        private void Die()
        {
            OnDied?.Invoke();
            OnDiedLocal?.Invoke();
            if (isPlayer) GameEvents.RaisePlayerDied();
        }
    }
}
