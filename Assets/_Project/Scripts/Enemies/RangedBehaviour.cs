using UnityEngine;

namespace BiomeWar
{
    // Fires pooled projectiles. Used by the Mummy and the stationary Snowman.
    public class RangedBehaviour : IEnemyBehaviour
    {
        EnemyContext ctx;
        float pendingFireTime = -1f;

        public void Initialise(EnemyContext context)
        {
            ctx = context;
        }

        public void Tick(float deltaTime)
        {
            if (pendingFireTime < 0f) return;

            pendingFireTime -= deltaTime;
            if (pendingFireTime > 0f) return;

            pendingFireTime = -1f;
            Fire();
        }

        public void Attack()
        {
            ctx.Animator?.PlayAttack();
            pendingFireTime = 0.25f;
        }

        void Fire()
        {
            if (ctx.ProjectilePrefab == null || !PoolManager.Exists) return;
            if (ctx.Target == null) return;

            Vector3 origin = ctx.FirePoint != null
                ? ctx.FirePoint.position
                : ctx.Self.position + Vector3.up * 1.2f;

            // Aim at chest height rather than the player's feet.
            Vector3 aim = ctx.Target.position;
            Vector3 dir = (aim - origin).normalized;

            GameObject go = PoolManager.Instance.Spawn(ctx.ProjectilePrefab, origin, Quaternion.LookRotation(dir));
            var proj = go.GetComponent<Projectile>();
            if (proj != null)
                proj.Launch(dir, ctx.Self.gameObject, ctx.AttackDamage, 14f);
        }

        public void Exit()
        {
            pendingFireTime = -1f;
        }
    }
}
