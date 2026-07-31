using UnityEngine;

namespace BiomeWar
{
    // Enables touch controls only on mobile, and applies platform performance settings.
    public class MobileControlsCanvas : MonoBehaviour
    {
        [SerializeField] GameObject controlsRoot;

        void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (controlsRoot != null) controlsRoot.SetActive(true);
            Application.targetFrameRate = 60;
            QualitySettings.shadowDistance = 25f;
#elif UNITY_WEBGL
            if (controlsRoot != null) controlsRoot.SetActive(false);
            Application.targetFrameRate = 60;
#else
            if (controlsRoot != null) controlsRoot.SetActive(false);
#endif
        }

        void OnEnable() => GameEvents.OnGameStateChanged += OnStateChanged;
        void OnDisable() => GameEvents.OnGameStateChanged -= OnStateChanged;

        void OnStateChanged(GameStateId id)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (controlsRoot != null)
                controlsRoot.SetActive(id == GameStateId.Playing);
#endif
        }
    }
}
