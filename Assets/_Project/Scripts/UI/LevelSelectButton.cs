using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BiomeWar
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] int levelIndex;

        [Header("Visuals")]
        [SerializeField] Button button;
        [SerializeField] Image thumbnail;
        [SerializeField] GameObject lockIcon;
        [SerializeField] GameObject completeIcon;
        [SerializeField] Image[] starImages;
        [SerializeField] Sprite starFilled;
        [SerializeField] Sprite starEmpty;
        [SerializeField] TextMeshProUGUI levelName;

        [Header("Locked appearance")]
        [SerializeField] Color unlockedTint = Color.white;
        [SerializeField] Color lockedTint = new Color(0.25f, 0.25f, 0.25f, 1f);

        void Start()
        {
            Refresh();
        }

        void OnEnable()
        {
            GameEvents.OnLevelUnlocked += OnUnlocked;
            Refresh();
        }

        void OnDisable()
        {
            GameEvents.OnLevelUnlocked -= OnUnlocked;
        }

        void OnUnlocked(int index)
        {
            if (index == levelIndex) Refresh();
        }

        void Refresh()
        {
            if (!SaveManager.Exists) return;

            bool unlocked = SaveManager.Instance.IsUnlocked(levelIndex);
            int stars = SaveManager.Instance.GetStars(levelIndex);

            var entry = SaveManager.Instance.Data.GetLevel(levelIndex);
            bool completed = entry != null && entry.Completed;

            if (button != null) button.interactable = unlocked;

            if (thumbnail != null)
            {
                thumbnail.color = unlocked ? unlockedTint : lockedTint;

                if (LevelManager.Exists)
                {
                    var config = LevelManager.Instance.GetLevel(levelIndex);
                    if (config != null && config.Thumbnail != null)
                        thumbnail.sprite = config.Thumbnail;
                    if (config != null && levelName != null)
                        levelName.text = config.LevelName;
                }
            }

            if (lockIcon != null) lockIcon.SetActive(!unlocked);
            if (completeIcon != null) completeIcon.SetActive(completed);

            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] == null) continue;
                starImages[i].gameObject.SetActive(unlocked);
                starImages[i].sprite = i < stars ? starFilled : starEmpty;
            }
        }

        public void OnClicked()
        {
            if (!LevelManager.Exists) return;
            LevelManager.Instance.LoadLevel(levelIndex);
        }
    }
}
