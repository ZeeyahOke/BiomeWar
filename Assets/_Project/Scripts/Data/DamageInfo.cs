using UnityEngine;

namespace BiomeWar
{
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
