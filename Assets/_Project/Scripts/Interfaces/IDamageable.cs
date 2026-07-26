using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Anything that can receive damage: player, enemies, destructible props.
    /// Lets the weapon system deal damage without knowing what it hit.
    /// </summary>
    public interface IDamageable
    {
        bool IsAlive { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void TakeDamage(DamageInfo info);
    }
}
