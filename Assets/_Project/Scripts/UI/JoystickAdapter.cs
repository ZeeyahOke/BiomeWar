using UnityEngine;

namespace BiomeWar
{
    public class JoystickAdapter : MonoBehaviour
    {
        [SerializeField] RectTransform thumb;
        [SerializeField] float movementRadius = 75f;
        [SerializeField] float deadzone = 0.15f;

        void Update()
        {
            if (!InputReader.Exists || thumb == null) return;

            Vector2 input = thumb.anchoredPosition / movementRadius;
            input = Vector2.ClampMagnitude(input, 1f);

            if (input.sqrMagnitude < deadzone * deadzone)
                input = Vector2.zero;

            InputReader.Instance.VirtualMove = input;
        }
    }
}
