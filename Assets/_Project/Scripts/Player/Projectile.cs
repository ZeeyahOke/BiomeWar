using UnityEngine;

namespace BiomeWar
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] float speed = 25f;
        [SerializeField] float lifetime = 4f;
        [SerializeField] float damage = 20f;
        [SerializeField] GameObject impactEffect;

        GameObject owner;
        float age;
        bool spent;

        public void Launch(Vector3 direction, GameObject shooter, float dmg, float spd)
        {
            owner = shooter;
            damage = dmg;
            speed = spd;
            transform.forward = direction.normalized;
            spent = false;
            age = 0f;
        }

        void Update()
        {
            if (spent) return;

            float step = speed * Time.deltaTime;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, step))
            {
                Hit(hit.collider, hit.point, hit.normal);
                return;
            }

            transform.position += transform.forward * step;

            age += Time.deltaTime;
            if (age >= lifetime) Despawn();
        }

        void Hit(Collider col, Vector3 point, Vector3 normal)
        {
            if (col.gameObject == owner) return;

            var target = col.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive)
            {
                target.TakeDamage(new DamageInfo(damage, point, transform.forward, owner));
            }

            if (impactEffect != null && PoolManager.Exists)
                PoolManager.Instance.Spawn(impactEffect, point, Quaternion.LookRotation(normal));

            Despawn();
        }

        void Despawn()
        {
            spent = true;
            var pooled = GetComponent<PooledObject>();
            if (pooled != null) pooled.ReturnToPool();
            else gameObject.SetActive(false);
        }

        public void OnSpawnFromPool()
        {
            age = 0f;
            spent = false;
        }

        public void OnReturnToPool()
        {
            owner = null;
        }
    }
}
