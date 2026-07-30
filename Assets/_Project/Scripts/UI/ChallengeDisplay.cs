using TMPro;
using UnityEngine;

namespace BiomeWar
{
    public class ChallengeDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] bool showDescription = true;

        void OnEnable()
        {
            if (DailyChallengeService.Exists)
                DailyChallengeService.Instance.OnChallengeLoaded += Refresh;

            Refresh();
        }

        void OnDisable()
        {
            if (DailyChallengeService.Exists)
                DailyChallengeService.Instance.OnChallengeLoaded -= Refresh;
        }

        void Refresh()
        {
            if (label == null || !DailyChallengeService.Exists) return;

            var c = DailyChallengeService.Instance.Active;
            bool online = DailyChallengeService.Instance.IsOnline;

            if (!online)
            {
                label.text = "TODAY'S CHALLENGE — Unavailable (offline)";
                return;
            }

            label.text = showDescription
                ? $"TODAY'S CHALLENGE — {c.name}: {c.description}"
                : $"TODAY'S CHALLENGE — {c.name}";
        }
    }
}
