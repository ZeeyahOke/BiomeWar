using UnityEngine;

namespace BiomeWar
{
    /// <summary>Data-driven enemy definition. New enemies need no code changes.</summary>
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "BiomeWar/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Identity")]
        public string EnemyName = "Enemy";
        public GameObject Prefab;

        [Header("Strategy")]
        public EnemyBehaviourType Behaviour = EnemyBehaviourType.Chase;
        public EnemyAnimatorType AnimatorDriver = EnemyAnimatorType.AnimatorDriven;

        [Header("Stats")]
        public float MaxHealth = 50f;
        public float MoveSpeed = 3f;
        public float TurnSpeed = 8f;
        public float AttackDamage = 10f;
        public float AttackRange = 2f;
        public float AttackCooldown = 1.5f;
        public float DetectionRange = 15f;

        [Header("Ranged")]
        public GameObject ProjectilePrefab;
        public float ProjectileSpeed = 12f;

        [Header("Defensive")]
        [Range(0f, 1f)] public float FrontalDamageReduction = 0.8f;
        public float BlockAngle = 90f;

        [Header("Rewards")]
        public int ScoreValue = 100;

        [Header("Audio")]
        public AudioClip AggroSound;
        public AudioClip AttackSound;
        public AudioClip DeathSound;
    }
}
