using UnityEngine;

namespace BiomeWar
{
    public class SupplyCrate : MonoBehaviour, IInteractable
    {
        [SerializeField] float healAmount = 35f;
        [SerializeField] bool singleUse = true;
        [SerializeField] AudioClip openSound;
        [SerializeField] GameObject openEffect;
        [SerializeField] Renderer[] renderersToDim;

        bool used;

        public string Prompt => used ? "Empty" : "Open supply crate";

        public bool CanInteract(GameObject actor)
        {
            if (used && singleUse) return false;

            var health = actor.GetComponent<Health>();
            return health != null && health.CurrentHealth < health.MaxHealth;
        }

        public void Interact(GameObject actor)
        {
            if (!CanInteract(actor)) return;

            var health = actor.GetComponent<Health>();
            health.Heal(healAmount);

            used = true;

            if (openSound != null)
                AudioSource.PlayClipAtPoint(openSound, transform.position);

            if (openEffect != null && PoolManager.Exists)
                PoolManager.Instance.Spawn(openEffect, transform.position + Vector3.up, Quaternion.identity);

            foreach (var r in renderersToDim)
                if (r != null) r.material.color = Color.gray;
        }
    }
}
