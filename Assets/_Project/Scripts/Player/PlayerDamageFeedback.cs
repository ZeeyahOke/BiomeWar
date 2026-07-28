using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    // Screen flash + shake when the player takes a hit.
    // The Image is assigned on Day 4 when the HUD exists; null is safe until then.
    public class PlayerDamageFeedback : MonoBehaviour
    {
        [SerializeField] Image bloodOverlay;
        [SerializeField] float flashAlpha = 0.55f;
        [SerializeField] float fadeSpeed = 1.8f;
        [SerializeField] Transform shakeTarget;
        [SerializeField] float shakeAmount = 0.08f;
        [SerializeField] float shakeTime = 0.12f;

        float currentAlpha;
        float shakeTimer;
        Vector3 shakeOrigin;

        void Awake()
        {
            if (shakeTarget != null) shakeOrigin = shakeTarget.localPosition;
            SetAlpha(0f);
        }

        void OnEnable()
        {
            GameEvents.OnPlayerDamaged += OnDamaged;
            GameEvents.OnPlayerDied += OnDied;
        }

        void OnDisable()
        {
            GameEvents.OnPlayerDamaged -= OnDamaged;
            GameEvents.OnPlayerDied -= OnDied;
        }

        void OnDamaged(DamageInfo info)
        {
            currentAlpha = flashAlpha;
            shakeTimer = shakeTime;
        }

        void OnDied()
        {
            currentAlpha = 0.8f;
        }

        void Update()
        {
            if (currentAlpha > 0f)
            {
                currentAlpha = Mathf.Max(0f, currentAlpha - fadeSpeed * Time.deltaTime);
                SetAlpha(currentAlpha);
            }

            if (shakeTimer > 0f && shakeTarget != null)
            {
                shakeTimer -= Time.deltaTime;
                shakeTarget.localPosition = shakeOrigin + Random.insideUnitSphere * shakeAmount;

                if (shakeTimer <= 0f)
                    shakeTarget.localPosition = shakeOrigin;
            }
        }

        void SetAlpha(float a)
        {
            if (bloodOverlay == null) return;
            Color c = bloodOverlay.color;
            c.a = a;
            bloodOverlay.color = c;
        }
    }
}
