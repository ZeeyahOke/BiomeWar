using UnityEngine;

namespace BiomeWar
{
    // Boss style. Alternates between guarding and a heavier counter-attack.
    // Frontal damage reduction is handled by Health's directional resistance.
    public class DefensiveBehaviour : IEnemyBehaviour
    {
        EnemyContext ctx;
        float pendingHitTime = -1f;
        int swingCount;

        public void Initialise(EnemyContext context)
        {
            ctx = context;
        }

        public void Tick(float deltaTime)
        {
            if (pendingHitTime < 0f) return;

            pendingHitTime -= deltaTime;
            if (pendingHitTime > 0f) return;

            pendingHitTime = -1f;
            TryLandHit();
        }

        public void Attack()
        {
            ctx.Animator?.PlayAttack();
            swingCount++;
            pendingHitTime = 0.4f;
        }

        void TryLandHit()
        {
            if (ctx.Target == null || ctx.TargetDamageable == null) return;
            if (!ctx.TargetDamageable.IsAlive) return;

            float dist = Vector3.Distance(ctx.Self.position, ctx.Target.position);
            if (dist > ctx.AttackRange * 1.4f) return;

            // Every third swing is a heavy hit.
            float damage = (swingCount % 3 == 0) ? ctx.AttackDamage * 2f : ctx.AttackDamage;

            Vector3 dir = (ctx.Target.position - ctx.Self.position).normalized;
            ctx.TargetDamageable.TakeDamage(
                new DamageInfo(damage, ctx.Target.position, dir, ctx.Self.gameObject));
        }

        public void Exit()
        {
            pendingHitTime = -1f;
        }
    }
}
