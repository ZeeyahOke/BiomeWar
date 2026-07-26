using UnityEngine;

namespace BiomeWar
{
     /// <summary>Singleton pattern base for manager MonoBehaviours.</summary>
    public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError($"[ManagerBase] {typeof(T).Name} requested but does not exist.");
                return _instance;
            }
        }

        public static bool Exists => _instance != null;

        [SerializeField] protected bool persistAcrossScenes = true;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;

            if (persistAcrossScenes)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
