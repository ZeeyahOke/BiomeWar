using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Strategy pattern contract for player abilities.
    /// The player holds IAbility references and never knows the concrete type,
    /// so new abilities can be added without modifying the player.
    /// </summary>
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
