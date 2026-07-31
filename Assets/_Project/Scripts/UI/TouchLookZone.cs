using UnityEngine;
using UnityEngine.EventSystems;

namespace BiomeWar
{
    public class TouchLookZone : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] float sensitivity = 1.5f;

        public void OnDrag(PointerEventData eventData)
        {
            if (InputReader.Exists)
                InputReader.Instance.VirtualLook = eventData.delta * sensitivity;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (InputReader.Exists)
                InputReader.Instance.VirtualLook = Vector2.zero;
        }
    }
}
