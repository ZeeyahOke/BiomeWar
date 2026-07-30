using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;

        void Start()
        {
            if (!SaveManager.Exists) return;

            var settings = SaveManager.Instance.Data.Settings;

            if (musicSlider != null)
            {
                musicSlider.value = settings.MusicVolume;
                musicSlider.onValueChanged.AddListener(OnMusicChanged);
            }

            if (sfxSlider != null)
            {
                sfxSlider.value = settings.SfxVolume;
                sfxSlider.onValueChanged.AddListener(OnSfxChanged);
            }
        }

        void OnMusicChanged(float value)
        {
            if (!SaveManager.Exists) return;
            SaveManager.Instance.Data.Settings.MusicVolume = value;
           // if (AudioManager.Exists) AudioManager.Instance.SetMusicVolume(value);
        }

        void OnSfxChanged(float value)
        {
            if (!SaveManager.Exists) return;
            SaveManager.Instance.Data.Settings.SfxVolume = value;
            //if (AudioManager.Exists) AudioManager.Instance.SetSfxVolume(value);
        }

        public void OnBack()
        {
            if (SaveManager.Exists) SaveManager.Instance.Save();
        }
    }
}
