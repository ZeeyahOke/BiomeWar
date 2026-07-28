using UnityEngine;

namespace BiomeWar
{
    //Damage lands only if the player is still in range when the swing connects.
    public class ChaseBehaviour : IEnemyBehaviour
    {
        EnemyContext ctx;
        float pendingHitTime = -1f;

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
            pendingHitTime = 0.35f;   // roughly when the swing peaks
        }

        void TryLandHit()
        {
            if (ctx.Target == null || ctx.TargetDamageable == null) return;
            if (!ctx.TargetDamageable.IsAlive) return;

            float dist = Vector3.Distance(ctx.Self.position, ctx.Target.position);
            if (dist > ctx.AttackRange * 1.3f) return;

            Vector3 dir = (ctx.Target.position - ctx.Self.position).normalized;
            ctx.TargetDamageable.TakeDamage(
                new DamageInfo(ctx.AttackDamage, ctx.Target.position, dir, ctx.Self.gameObject));
        }

        public void Exit()
        {
            pendingHitTime = -1f;
        }
    }
}
