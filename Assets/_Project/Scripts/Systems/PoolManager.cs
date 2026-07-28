using System.Collections.Generic;
using UnityEngine;

namespace BiomeWar
{
    /// <summary>Singleton registry of pools, keyed by prefab.</summary>
    public class PoolManager : ManagerBase<PoolManager>
    {
        [SerializeField] private int defaultPoolSize = 20;

        private readonly Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();

        public ObjectPool GetPool(GameObject prefab, int initialSize = -1)
        {
            if (prefab == null) return null;

            if (!_pools.TryGetValue(prefab, out var pool))
            {
                var holder = new GameObject($"Pool_{prefab.name}");
                holder.transform.SetParent(transform);

                pool = new ObjectPool(prefab, initialSize < 0 ? defaultPoolSize : initialSize, holder.transform);
                _pools.Add(prefab, pool);
            }

            return pool;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return GetPool(prefab)?.Get(position, rotation);
        }
    }
}
