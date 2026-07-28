using UnityEngine;

namespace BiomeWar
{
    // Radial damage around the player. Uses OverlapSphere then filters by IDamageable.
    public class SlamAbility : AbilityBase
    {
        [SerializeField] float radius = 6f;
        [SerializeField] float damage = 45f;
        [SerializeField] LayerMask targetLayers = ~0;
        [SerializeField] GameObject slamEffect;

        readonly Collider[] hits = new Collider[32];

        public override void Initialise(GameObject ownerObject)
        {
            base.Initialise(ownerObject);
            displayName = "Slam";
        }

        protected override void Execute()
        {
            Vector3 origin = owner.transform.position;

            if (slamEffect != null && PoolManager.Exists)
                PoolManager.Instance.Spawn(slamEffect, origin, Quaternion.identity);

            int count = Physics.OverlapSphereNonAlloc(origin, radius, hits, targetLayers);

            for (int i = 0; i < count; i++)
            {
                if (hits[i].gameObject == owner) continue;

                var target = hits[i].GetComponentInParent<IDamageable>();
                if (target == null || !target.IsAlive) continue;

                Vector3 dir = (hits[i].transform.position - origin).normalized;
                target.TakeDamage(new DamageInfo(damage, hits[i].transform.position, dir, owner));
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
