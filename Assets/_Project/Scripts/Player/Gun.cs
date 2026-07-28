using UnityEngine;

namespace BiomeWar
{
    public class Gun : MonoBehaviour
    {
        [Header("Firing")]
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform muzzle;
        [SerializeField] float fireRate = 6f;
        [SerializeField] float damage = 20f;
        [SerializeField] float projectileSpeed = 30f;

        [Header("Feel")]
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] AudioSource fireSound;

        float nextFireTime;
        Camera cam;
        bool canFire = true;

        void Awake()
        {
            cam = Camera.main;
            if (muzzle == null) muzzle = transform;
        }

        void OnEnable()
        {
            GameEvents.OnGameStateChanged += OnStateChanged;
            GameEvents.OnPlayerDied += OnDied;
        }

        void OnDisable()
        {
            GameEvents.OnGameStateChanged -= OnStateChanged;
            GameEvents.OnPlayerDied -= OnDied;
        }

        void OnStateChanged(GameStateId id) => canFire = (id == GameStateId.Playing);
        void OnDied() => canFire = false;

        void Update()
        {
            if (!canFire || !InputReader.Exists) return;

            if (InputReader.Instance.Fire && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }

        void Fire()
        {
            if (projectilePrefab == null || !PoolManager.Exists) return;

            Vector3 dir = AimDirection();

            GameObject go = PoolManager.Instance.Spawn(projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
            var proj = go.GetComponent<Projectile>();
            if (proj != null)
                proj.Launch(dir, transform.root.gameObject, damage, projectileSpeed);

            if (muzzleFlash != null) muzzleFlash.Play();
            if (fireSound != null) fireSound.Play();
        }

        // Aim at whatever the crosshair is over, not straight out of the barrel.
        Vector3 AimDirection()
        {
            if (cam == null) return muzzle.forward;

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Vector3 aimPoint = Physics.Raycast(ray, out RaycastHit hit, 200f)
                ? hit.point
                : ray.GetPoint(200f);

            return (aimPoint - muzzle.position).normalized;
        }
    }
}
