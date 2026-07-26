using UnityEngine;

namespace BiomeWar
{
    public interface IAbility
    {
        string DisplayName { get; }
        Sprite Icon { get; }
        float Cooldown { get; }
        float CooldownRemaining { get; }
        bool IsReady { get; }

        void Initialise(GameObject owner);
        void Activate();
        void Tick(float deltaTime);
    }
}
