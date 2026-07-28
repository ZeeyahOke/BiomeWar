using UnityEngine;

namespace BiomeWar
{
    /// <summary>Pure damage maths. No Unity dependencies — directly unit testable.</summary>
    public static class DamageCalculator
    {
        public const float MinDamage = 0f;

        public static float Calculate(float baseDamage, float resistance01, float distance,
                                      float falloffStart, float falloffEnd, float minMultiplier = 0.25f)
        {
            if (baseDamage <= 0f) return MinDamage;

            float resist = Mathf.Clamp01(resistance01);
            float afterResist = baseDamage * (1f - resist);
            float falloff = CalculateFalloff(distance, falloffStart, falloffEnd, minMultiplier);

            return Mathf.Max(MinDamage, afterResist * falloff);
        }

        public static float CalculateFalloff(float distance, float falloffStart, float falloffEnd, float minMultiplier)
        {
            if (falloffEnd <= falloffStart) return 1f;
            if (distance <= falloffStart) return 1f;
            if (distance >= falloffEnd) return minMultiplier;

            float t = (distance - falloffStart) / (falloffEnd - falloffStart);
            return Mathf.Lerp(1f, minMultiplier, t);
        }

        /// <summary>Directional resistance: reduced damage from the front only.</summary>
        public static float DirectionalResistance(Vector3 defenderForward, Vector3 hitDirection,
                                                  float blockAngleDegrees, float frontalReduction)
        {
            Vector3 toAttacker = -hitDirection.normalized;
            float angle = Vector3.Angle(defenderForward.normalized, toAttacker);
            return angle <= blockAngleDegrees * 0.5f ? Mathf.Clamp01(frontalReduction) : 0f;
        }
    }
}
