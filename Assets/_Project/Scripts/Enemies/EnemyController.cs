using UnityEngine;

namespace BiomeWar
{
    [RequireComponent(typeof(Health))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] EnemyConfig config;
        [SerializeField] Transform firePoint;
        [SerializeField] bool isBoss;

        Health health;
        AudioSource audioSource;
        IEnemyBehaviour behaviour;
        IEnemyAnimator enemyAnimator;
        EnemyContext ctx;
        readonly StateMachine states = new StateMachine();

        IdleState idle;
        ChaseState chase;
        AttackState attack;
        DeadState dead;

        bool isDead;

        public EnemyConfig Config => config;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            if (config == null)
            {
                Debug.LogError($"{name} has no EnemyConfig assigned.");
                enabled = false;
                return;
            }

            Setup();
        }

        void Setup()
        {
            enemyAnimator = GetComponent<IEnemyAnimator>();

            var player = GameObject.FindGameObjectWithTag("Player");

            ctx = new EnemyContext
            {
                Self = transform,
                Target = player != null ? player.transform : null,
                TargetDamageable = player != null ? player.GetComponent<IDamageable>() : null,
                Animator = enemyAnimator,
                MoveSpeed = config.MoveSpeed,
                TurnSpeed = config.TurnSpeed,
                AttackRange = config.AttackRange,
                AttackDamage = config.AttackDamage,
                AttackCooldown = config.AttackCooldown,
                DetectionRange = config.DetectionRange,
                ProjectilePrefab = config.ProjectilePrefab,
                FirePoint = firePoint
            };

            bool defensive = config.Behaviour == EnemyBehaviourType.Defensive;
            health.Configure(config.MaxHealth, defensive, config.FrontalDamageReduction, config.BlockAngle);

            behaviour = EnemyBehaviourFactory.Create(config.Behaviour);
            behaviour.Initialise(ctx);

            idle = new IdleState(this, ctx);
            chase = new ChaseState(this, ctx);
            attack = new AttackState(this, ctx);
            dead = new DeadState(this, ctx);

            health.OnDamaged += OnDamaged;
            health.OnDied += OnDied;

            if (isBoss)
            {
                GameEvents.RaiseBossSpawned(gameObject);
                GameEvents.RaiseBossHealthChanged(health.CurrentHealth, health.MaxHealth);
            }

            states.ChangeState(idle);
        }

        void Update()
        {
            if (isDead) return;

            float dt = Time.deltaTime;
            states.Tick(dt);
            behaviour?.Tick(dt);
        }

        // Stationary enemies (the Snowman) never chase.
        public void EnterIdle() => states.ChangeState(idle);

        public void EnterChase()
        {
            if (config.MoveSpeed <= 0.01f)
            {
                states.ChangeState(attack);
                return;
            }
            states.ChangeState(chase);
        }

        public void EnterAttack() => states.ChangeState(attack);

        public void PerformAttack()
        {
            behaviour?.Attack();
            PlayClip(config.AttackSound);
        }

        void OnDamaged(DamageInfo info)
        {
                if (isDead || !health.IsAlive) return;

                enemyAnimator?.PlayHit();

            if (isBoss)
                GameEvents.RaiseBossHealthChanged(health.CurrentHealth, health.MaxHealth);

            // Wake up if shot from outside detection range.
            if (states.CurrentState == idle)
                EnterChase();
        }

        void OnDied()
        {
            if (isDead) return;
            isDead = true;

            behaviour?.Exit();
            states.ChangeState(dead);

            PlayClip(config.DeathSound);
            GameEvents.RaiseEnemyDefeated(gameObject);

            foreach (var col in GetComponentsInChildren<Collider>())
                col.enabled = false;

            Destroy(gameObject, 6f);
        }

        void PlayClip(AudioClip clip)
        {
            if (clip == null || audioSource == null) return;
            audioSource.PlayOneShot(clip);
        }

        void OnDestroy()
        {
            if (health == null) return;
            health.OnDamaged -= OnDamaged;
            health.OnDied -= OnDied;
        }

        void OnDrawGizmosSelected()
        {
            if (config == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, config.DetectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, config.AttackRange);
        }

        public void MarkAsBoss()
        {
            isBoss = true;
        }
    }
}
