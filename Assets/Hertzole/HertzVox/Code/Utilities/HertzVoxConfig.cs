namespace Hertzole.HertzVox
{
    public class HertzVoxConfig
    {
        // Multithreading must be disabled on web builds
#if !UNITY_WEBGL
        private static bool useMultiThreading = true;
#else
        private static bool useMultiThreading = false;
#endif
        public static bool UseMultiThreading { get { return useMultiThreading; } }

        public static float BlockSize { get { return 1f; } }
        public static float AOStrength { get { return 0.6f; } }
    }
}
