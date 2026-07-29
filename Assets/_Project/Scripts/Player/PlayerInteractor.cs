using UnityEngine;

namespace BiomeWar
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] Camera viewCamera;
        [SerializeField] float range = 3.5f;
        [SerializeField] LayerMask interactMask = ~0;

        IInteractable current;

        public IInteractable Current => current;
        public string CurrentPrompt => current != null ? current.Prompt : string.Empty;

        void Awake()
        {
            if (viewCamera == null) viewCamera = Camera.main;
        }

        void Update()
        {
            if (GameManager.Exists && GameManager.Instance.CurrentStateId != GameStateId.Playing)
            {
                current = null;
                return;
            }

            Scan();

            if (current != null && InputReader.Exists && InputReader.Instance.Interact)
                current.Interact(gameObject);
        }

        void Scan()
        {
            current = null;
            if (viewCamera == null) return;

            Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, range, interactMask)) return;

                Debug.Log($"Ray hit: {hit.collider.name}");

            var candidate = hit.collider.GetComponentInParent<IInteractable>();
            if (candidate != null && candidate.CanInteract(gameObject))
                current = candidate;
                
        }
    }
}
