using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Everything a behaviour Strategy needs to operate, bundled into one object.
    /// Behaviours receive this on Initialise, so they never reach back into
    /// the enemy controller or any concrete Unity component.
    /// </summary>
    public class EnemyContext
    {
        public Transform Self;
        public Transform Target;
        public IEnemyAnimator Animator;
        public IDamageable TargetDamageable;

        public float MoveSpeed = 3f;
        public float TurnSpeed = 8f;
        public float AttackRange = 2f;
        public float AttackDamage = 10f;
        public float AttackCooldown = 1.5f;
        public float DetectionRange = 15f;

        public GameObject ProjectilePrefab;
        public Transform FirePoint;
    }
}
