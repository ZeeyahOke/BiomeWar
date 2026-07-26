using UnityEngine;

namespace BiomeWar
{
    /// <summary>
    /// Items that contribute to level completion star rating or inventory.
    /// </summary>
    public interface ICollectable
    {
        string CollectableId { get; }
        bool IsCollected { get; }
        void Collect(GameObject collector);
    }
}
