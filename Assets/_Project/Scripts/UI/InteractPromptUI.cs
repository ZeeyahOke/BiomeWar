using TMPro;
using UnityEngine;

namespace BiomeWar
{
    public class InteractPromptUI : MonoBehaviour
    {
        [SerializeField] PlayerInteractor interactor;
        [SerializeField] TextMeshProUGUI label;

        void Update()
        {
            if (interactor == null || label == null) return;

            string prompt = interactor.CurrentPrompt;
            bool show = !string.IsNullOrEmpty(prompt);

            if (label.gameObject.activeSelf != show)
                label.gameObject.SetActive(show);

            if (!show) return;

#if UNITY_ANDROID || UNITY_IOS
            label.text = prompt;
#else
            label.text = $"[E] {prompt}";
#endif
        }
    }
}
