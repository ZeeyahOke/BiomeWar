using UnityEngine;

namespace BiomeWar
{
    public class Collectible : MonoBehaviour, ICollectable, IInteractable
    {
        [SerializeField] string collectableId = "relic_01";
        [SerializeField] string prompt = "Pick up relic";
        [SerializeField] bool autoCollectOnTouch = true;
        [SerializeField] float spinSpeed = 60f;
        [SerializeField] float bobHeight = 0.2f;
        [SerializeField] float bobSpeed = 2f;
        [SerializeField] AudioClip collectSound;
        [SerializeField] GameObject collectEffect;

        Vector3 startPos;
        float bobTimer;
        bool collected;

        public string CollectableId => collectableId;
        public bool IsCollected => collected;
        public string Prompt => prompt;

        void Awake() => startPos = transform.position;

        void Update()
        {
            if (collected) return;

            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

            bobTimer += Time.deltaTime * bobSpeed;
            transform.position = startPos + Vector3.up * Mathf.Sin(bobTimer) * bobHeight;
        }

        public bool CanInteract(GameObject actor) => !collected;

        public void Interact(GameObject actor) => Collect(actor);

        public void Collect(GameObject collector)
        {
            if (collected) return;
            collected = true;

            GameEvents.RaiseItemCollected(collectableId);

            if (collectEffect != null && PoolManager.Exists)
                PoolManager.Instance.Spawn(collectEffect, transform.position, Quaternion.identity);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!autoCollectOnTouch || collected) return;
            if (!other.CompareTag("Player")) return;
            Collect(other.gameObject);
        }
    }
}
