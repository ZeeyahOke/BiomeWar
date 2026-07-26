using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Anything the player can interact with via the interaction raycast.
    /// Implemented by supply crates, collectibles, and any future object.
    /// </summary>
    public interface IInteractable
    {
        string Prompt { get; }
        bool CanInteract(GameObject actor);
        void Interact(GameObject actor);
    }
}
