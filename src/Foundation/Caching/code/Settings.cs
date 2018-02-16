namespace Lotus.Foundation.Caching
{
    internal static class Settings
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