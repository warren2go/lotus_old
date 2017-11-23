namespace Lotus.Foundation.Caching.Configuration
{
    public static class CachingSettings
    {
        internal static bool Enabled
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.Caching.Enabled", false);
            }
        }
    }
}