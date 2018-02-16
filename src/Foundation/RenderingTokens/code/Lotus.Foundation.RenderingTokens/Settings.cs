namespace Lotus.Foundation.RenderingTokens
{
    internal static class Settings
    {
        internal static bool Enabled
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.Enabled", false);
            }
        }
        
        internal static bool IsDebug
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.IsDebug", true);
            }
        }
        
        internal static bool ForceReplace
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.ForceReplace", true);
            }
        }
        
        internal static string ExtractPattern
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ExtractPattern", "$(.+?)[.]?([a-zA-Z0-9()])?");
            }
        }
        
        internal static string ResolveFormat
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ResolveFormat", "{0}.{1}");
            }
        }
    }
}