using UnityEngine;

namespace BiomeWar
{
    public class AudioManager : ManagerBase<AudioManager>
    {
        [Header("Sources")]
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] AudioClip menuMusic;

        [Header("One-shots")]
        [SerializeField] AudioClip levelCompleteSound;
        [SerializeField] AudioClip gameOverSound;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            if (musicSource != null) musicSource.loop = true;
            ApplySavedVolumes();
        }

        void OnEnable()
        {
            GameEvents.OnLevelStarted += OnLevelStarted;
            GameEvents.OnLevelCompleted += OnLevelCompleted;
            GameEvents.OnPlayerDied += OnPlayerDied;
            GameEvents.OnGameStateChanged += OnStateChanged;
        }

        void OnDisable()
        {
            GameEvents.OnLevelStarted -= OnLevelStarted;
            GameEvents.OnLevelCompleted -= OnLevelCompleted;
            GameEvents.OnPlayerDied -= OnPlayerDied;
            GameEvents.OnGameStateChanged -= OnStateChanged;
        }

        void Start() => PlayMusic(menuMusic);

        void ApplySavedVolumes()
        {
            if (!SaveManager.Exists) return;

            var s = SaveManager.Instance.Data.Settings;
            SetMusicVolume(s.MusicVolume);
            SetSfxVolume(s.SfxVolume);
        }

        public void SetMusicVolume(float value)
        {
            if (musicSource != null) musicSource.volume = Mathf.Clamp01(value);
        }

        public void SetSfxVolume(float value)
        {
            if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(value);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip);
        }

        void OnLevelStarted(int levelIndex)
        {
            if (!LevelManager.Exists) return;

            var config = LevelManager.Instance.GetLevel(levelIndex);
            if (config != null && config.MusicTrack != null)
                PlayMusic(config.MusicTrack);
        }

        void OnLevelCompleted(LevelResult result)
        {
            if (musicSource != null) musicSource.Stop();
            PlaySfx(levelCompleteSound);
        }

        void OnPlayerDied()
        {
            if (musicSource != null) musicSource.Stop();
            PlaySfx(gameOverSound);
        }

        // Returning to the menu restores menu music.
        void OnStateChanged(GameStateId id)
        {
            if (id == GameStateId.MainMenu) PlayMusic(menuMusic);
        }
    }
}
