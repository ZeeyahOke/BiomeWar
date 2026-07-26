namespace BiomeWar
{
    /// <summary>
    /// Objects managed by the generic ObjectPool. Lets pooled objects reset
    /// their own state instead of the pool needing to know their internals.
    /// </summary>
    public interface IPoolable
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
    }
}
