using System.Collections.Generic;
using UnityEngine;

namespace BiomeWar
{
    /// <summary>Object Pooling pattern. Reuses instances instead of Instantiate/Destroy.</summary>
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _available = new Queue<GameObject>();
        private readonly List<GameObject> _all = new List<GameObject>();
        private readonly int _maxSize;

        public int TotalCreated => _all.Count;
        public int AvailableCount => _available.Count;

        public ObjectPool(GameObject prefab, int initialSize, Transform parent = null, int maxSize = 200)
        {
            _prefab = prefab;
            _parent = parent;
            _maxSize = maxSize;

            for (int i = 0; i < initialSize; i++)
                _available.Enqueue(CreateNew());
        }

        private GameObject CreateNew()
        {
            GameObject go = Object.Instantiate(_prefab, _parent);
            go.SetActive(false);

            var pooled = go.GetComponent<PooledObject>();
            if (pooled == null) pooled = go.AddComponent<PooledObject>();
            pooled.ReturnAction = Return;

            _all.Add(go);
            return go;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject go;

            if (_available.Count > 0)
            {
                go = _available.Dequeue();
            }
            else if (_all.Count < _maxSize)
            {
                go = CreateNew();
            }
            else
            {
                go = _all[0];
                _all.RemoveAt(0);
                _all.Add(go);
            }

            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);

            foreach (var p in go.GetComponents<IPoolable>()) p.OnSpawnFromPool();
            return go;
        }

        public void Return(GameObject go)
        {
            if (go == null || !go.activeSelf) return;

            foreach (var p in go.GetComponents<IPoolable>()) p.OnReturnToPool();

            go.SetActive(false);
            if (_parent != null) go.transform.SetParent(_parent);
            _available.Enqueue(go);
        }
    }
}
