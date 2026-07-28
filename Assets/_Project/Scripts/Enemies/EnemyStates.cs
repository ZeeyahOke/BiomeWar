using UnityEngine;

namespace BiomeWar
{
    public abstract class EnemyStateBase : IState
    {
        protected readonly EnemyContext Ctx;
        protected readonly EnemyController Enemy;

        protected EnemyStateBase(EnemyController enemy, EnemyContext ctx)
        {
            Enemy = enemy;
            Ctx = ctx;
        }

        public virtual void Enter() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Exit() { }

        protected float DistanceToTarget()
        {
            if (Ctx.Target == null) return float.MaxValue;
            return Vector3.Distance(Ctx.Self.position, Ctx.Target.position);
        }

        protected bool TargetAlive()
        {
            return Ctx.TargetDamageable != null && Ctx.TargetDamageable.IsAlive;
        }
    }

    // Waits until the player comes within detection range.
    public class IdleState : EnemyStateBase
    {
        public IdleState(EnemyController e, EnemyContext c) : base(e, c) { }

        public override void Enter() => Ctx.Animator?.PlayMove(false);

        public override void Tick(float deltaTime)
        {
            if (!TargetAlive()) return;

            if (DistanceToTarget() <= Ctx.DetectionRange)
                Enemy.EnterChase();
        }
    }

    // Moves toward the player until in attack range.
    public class ChaseState : EnemyStateBase
    {
        public ChaseState(EnemyController e, EnemyContext c) : base(e, c) { }

        public override void Enter() => Ctx.Animator?.PlayMove(true);
        public override void Exit() => Ctx.Animator?.PlayMove(false);

        public override void Tick(float deltaTime)
        {
            if (!TargetAlive())
            {
                Enemy.EnterIdle();
                return;
            }

            float dist = DistanceToTarget();

            if (dist <= Ctx.AttackRange)
            {
                Enemy.EnterAttack();
                return;
            }

            if (dist > Ctx.DetectionRange * 1.5f)
            {
                Enemy.EnterIdle();
                return;
            }

            MoveTowardTarget(deltaTime);
        }

        void MoveTowardTarget(float deltaTime)
        {
            Vector3 to = Ctx.Target.position - Ctx.Self.position;
            to.y = 0f;

            if (to.sqrMagnitude < 0.01f) return;

            Vector3 dir = to.normalized;

            Quaternion want = Quaternion.LookRotation(dir);
            Ctx.Self.rotation = Quaternion.Slerp(Ctx.Self.rotation, want, Ctx.TurnSpeed * deltaTime);

            Ctx.Self.position += dir * Ctx.MoveSpeed * deltaTime;
        }
    }

    // Attacks on a cooldown while the player stays in range.
    public class AttackState : EnemyStateBase
    {
        float cooldownLeft;

        public AttackState(EnemyController e, EnemyContext c) : base(e, c) { }

        public override void Enter()
        {
            Ctx.Animator?.PlayMove(false);
            cooldownLeft = 0f;
        }

        public override void Tick(float deltaTime)
        {
            if (!TargetAlive())
            {
                Enemy.EnterIdle();
                return;
            }

            FaceTarget(deltaTime);

            if (DistanceToTarget() > Ctx.AttackRange * 1.2f)
            {
                Enemy.EnterChase();
                return;
            }

            cooldownLeft -= deltaTime;
            if (cooldownLeft <= 0f)
            {
                cooldownLeft = Ctx.AttackCooldown;
                Enemy.PerformAttack();
            }
        }

        void FaceTarget(float deltaTime)
        {
            Vector3 to = Ctx.Target.position - Ctx.Self.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.01f) return;

            Quaternion want = Quaternion.LookRotation(to.normalized);
            Ctx.Self.rotation = Quaternion.Slerp(Ctx.Self.rotation, want, Ctx.TurnSpeed * deltaTime);
        }
    }

    // Terminal state. Nothing transitions out of it.
    public class DeadState : EnemyStateBase
    {
        public DeadState(EnemyController e, EnemyContext c) : base(e, c) { }

        public override void Enter()
        {
            Ctx.Animator?.PlayDeath();
        }
    }
}
