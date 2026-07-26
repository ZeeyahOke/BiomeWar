using UnityEngine;

namespace BiomeWar
{
    public interface ICollectable
    {
        string CollectableId { get; }
        bool IsCollected { get; }
        void Collect(GameObject collector);
    }
}
