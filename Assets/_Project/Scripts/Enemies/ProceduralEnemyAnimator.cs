using UnityEngine;

namespace BiomeWar
{
    // For the Snowman, which has no skeleton. Animates the model's parts directly.
    public class ProceduralEnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        [Header("Model parts")]
        [SerializeField] Transform body;
        [SerializeField] Transform head;
        [SerializeField] Transform hat;
        [SerializeField] Transform rightArm;

        [Header("Idle")]
        [SerializeField] float bobHeight = 0.06f;
        [SerializeField] float bobSpeed = 1.6f;

        [Header("Attack")]
        [SerializeField] float swingAngle = 70f;
        [SerializeField] float swingDuration = 0.35f;

        [Header("Death")]
        [SerializeField] float fallDuration = 0.6f;
        [SerializeField] float sinkDepth = 1.5f;

        Vector3 headStart;
        Quaternion armStart;
        float bobTimer;
        float swingTimer;
        float hitFlashTimer;
        bool dying;
        float deathTimer;
        Quaternion deathStartRot;
        Vector3 deathStartPos;

        void Awake()
        {
            if (body == null) body = transform;
            if (head != null) headStart = head.localPosition;
            if (rightArm != null) armStart = rightArm.localRotation;
        }

        void Update()
        {
            if (dying)
            {
                TickDeath();
                return;
            }

            TickIdleBob();
            TickSwing();
            TickHitFlash();
        }

        void TickIdleBob()
        {
            if (head == null) return;
            bobTimer += Time.deltaTime * bobSpeed;
            head.localPosition = headStart + Vector3.up * Mathf.Sin(bobTimer) * bobHeight;
        }

        void TickSwing()
        {
            if (rightArm == null || swingTimer <= 0f) return;

            swingTimer -= Time.deltaTime;
            float t = 1f - (swingTimer / swingDuration);

            // Out and back: peaks at the halfway point.
            float curve = Mathf.Sin(t * Mathf.PI);
            rightArm.localRotation = armStart * Quaternion.Euler(-swingAngle * curve, 0f, 0f);

            if (swingTimer <= 0f) rightArm.localRotation = armStart;
        }

        void TickHitFlash()
        {
            if (hitFlashTimer <= 0f) return;
            hitFlashTimer -= Time.deltaTime;

            float squash = 1f + Mathf.Sin(hitFlashTimer * 30f) * 0.05f;
            body.localScale = new Vector3(squash, 2f - squash, squash);

            if (hitFlashTimer <= 0f) body.localScale = Vector3.one;
        }

        void TickDeath()
        {
            deathTimer += Time.deltaTime;
            float t = Mathf.Clamp01(deathTimer / fallDuration);

            transform.rotation = Quaternion.Slerp(deathStartRot, deathStartRot * Quaternion.Euler(90f, 0f, 0f), t);

            if (t >= 1f)
            {
                float sinkT = (deathTimer - fallDuration) * 0.5f;
                transform.position = deathStartPos + Vector3.down * Mathf.Min(sinkDepth, sinkT);
            }
        }

        public void PlayMove(bool isMoving) { }   // stationary turret

        public void PlayAttack()
        {
            swingTimer = swingDuration;
        }

        public void PlayHit()
        {
            hitFlashTimer = 0.2f;
        }

        public void PlayDeath()
        {
            if (dying) return;
            dying = true;
            deathTimer = 0f;
            deathStartRot = transform.rotation;
            deathStartPos = transform.position;

            if (hat != null)
            {
                hat.SetParent(null);
                var rb = hat.gameObject.AddComponent<Rigidbody>();
                rb.AddForce(Vector3.up * 3f + Random.insideUnitSphere * 2f, ForceMode.Impulse);
                Destroy(hat.gameObject, 5f);
            }
        }
    }
}
