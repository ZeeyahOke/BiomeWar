using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Carries all context about a damage event. Passing a struct instead of a
    /// float means new fields (crit, element, knockback) can be added later
    /// without changing every IDamageable implementation.
    /// </summary>
    public struct DamageInfo
    {
        public float Amount;
        public Vector3 HitPoint;
        public Vector3 HitDirection;
        public GameObject Source;

        public DamageInfo(float amount, Vector3 hitPoint, Vector3 hitDirection, GameObject source)
        {
            Amount = amount;
            HitPoint = hitPoint;
            HitDirection = hitDirection;
            Source = source;
        }

        public static DamageInfo Simple(float amount, GameObject source = null)
        {
            return new DamageInfo(amount, Vector3.zero, Vector3.zero, source);
        }
    }
}
