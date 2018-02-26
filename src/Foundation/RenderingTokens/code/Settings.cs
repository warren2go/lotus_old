using System.Linq;

namespace Lotus.Foundation.RenderingTokens
{
    internal static class Settings
    {
        internal static bool InvokeEnabled
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.InvokeEnabled", true);
            }
        }

        private static string[] _invokeWhitelist;
        internal static string[] InvokeWhitelist
        {
            get
            {
                if (_invokeWhitelist == null)
                {
                    _invokeWhitelist = Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.InvokeWhitelist").Split('|').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                }
                return _invokeWhitelist;
            }
        }
        
        internal static bool ReplaceEnabled
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.ReplaceEnabled", true);
            }
        }
        
        internal static bool IsDebug
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.IsDebug", true);
            }
        }
        
        internal static bool ParameterCheck
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.RenderingTokens.ParameterCheck", false);
            }
        }

        internal static string ParameterKey
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.RenderingTokens.ParameterKey");
            }
        }
    }
}