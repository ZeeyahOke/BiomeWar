using UnityEngine;

namespace BiomeWar
{
    // Shared cooldown logic so each ability only writes its own effect.
    public abstract class AbilityBase : MonoBehaviour, IAbility
    {
        [SerializeField] protected string displayName = "Ability";
        [SerializeField] protected Sprite icon;
        [SerializeField] protected float cooldown = 5f;

        protected GameObject owner;
        float remaining;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public float Cooldown => cooldown;
        public float CooldownRemaining => remaining;
        public bool IsReady => remaining <= 0f;

        public virtual void Initialise(GameObject ownerObject)
        {
            owner = ownerObject;
        }

        public void Activate()
        {
            if (!IsReady) return;
            remaining = cooldown;
            Execute();
        }

        public virtual void Tick(float deltaTime)
        {
            if (remaining > 0f)
                remaining = Mathf.Max(0f, remaining - deltaTime);
        }

        protected abstract void Execute();
    }
}
