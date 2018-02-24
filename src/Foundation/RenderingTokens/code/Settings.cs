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
        
        internal static string DefaultExtractPattern
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.DefaultExtractPattern", @"\$\(.+\)(?:[.]?([a-zA-Z0-9_()]+)?)?");
            }
        }
        
        internal static string ResolveTokenFormat
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ResolveTokenFormat", "token:{0}");
            }
        }

        internal static class ParameterKeys
        {
            internal static string Tokens
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ParameterKeys.Tokens", "lotusRenderingTokens");
                }
            }
        
            internal static string ExtractPattern
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ParameterKeys.ExtractPattern", "lotusRenderingExtractPattern");
                }
            }   
        }
    }
}