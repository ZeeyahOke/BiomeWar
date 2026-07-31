using UnityEngine;
using UnityEngine.EventSystems;

namespace BiomeWar
{
    public enum TouchAction { Fire, Interact, Ability1, Ability2 }

    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] TouchAction action;
        [SerializeField] bool holdable;   // Fire is held; the rest are single taps

        public void OnPointerDown(PointerEventData eventData)
        {
            if (InputReader.Exists) Set(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (InputReader.Exists && holdable) Set(false);
        }

        void Set(bool value)
        {
            var r = InputReader.Instance;

            switch (action)
            {
                case TouchAction.Fire:     r.VirtualFire = value; break;
                case TouchAction.Interact: r.VirtualInteract = value; break;
                case TouchAction.Ability1: r.VirtualAbility1 = value; break;
                case TouchAction.Ability2: r.VirtualAbility2 = value; break;
            }
        }
    }
}
