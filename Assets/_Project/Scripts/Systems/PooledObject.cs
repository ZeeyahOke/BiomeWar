using UnityEngine;

namespace BiomeWar
{
    /// <summary>Attached to pooled prefabs so they can return themselves.</summary>
    public class PooledObject : MonoBehaviour
    {
        public System.Action<GameObject> ReturnAction;

        public void ReturnToPool()
        {
            if (ReturnAction != null) ReturnAction.Invoke(gameObject);
            else Destroy(gameObject);
        }

        public void ReturnAfter(float seconds)
        {
            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), seconds);
        }
    }
}
