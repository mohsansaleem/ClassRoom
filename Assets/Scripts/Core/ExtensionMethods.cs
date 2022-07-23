using PG.Core.Installer;

namespace PG.Core
{
    public static class ExtensionMethods
    {
        public static void Fire<T>(this T signal) where T : Signal
        {
            signal.SignalBus.Fire(signal);
        }
    }
}