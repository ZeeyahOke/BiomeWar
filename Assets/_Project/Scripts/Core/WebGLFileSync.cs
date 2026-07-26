using System.Runtime.InteropServices;

namespace BiomeWar
{
    /// <summary>WebGL only: flushes the virtual filesystem to IndexedDB.</summary>
    public static class WebGLFileSync
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        public static void Sync() => SyncFiles();
#else
        public static void Sync() { }
#endif
    }
}
