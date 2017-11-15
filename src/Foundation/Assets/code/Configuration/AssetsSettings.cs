using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotus.Foundation.Assets.Configuration
{
    internal static class AssetsSettings
    {
        internal static bool Enabled
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.Assets.Enabled", false);
            }
        }

        internal static string IgnoreType
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.IgnoreType", "querystring");
            }
        }

        internal static class CDN
        {
            internal static string Prefix
            {
                get { return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.CDN.Prefix", string.Empty); }
            }   
        }

        internal static class Caching
        {
            internal static int DefaultExpireHours
            {
                get { return Sitecore.Configuration.Settings.GetIntSetting("Lotus.Foundation.Assets.Caching.DefaultExpireHours", 0); }
            }
        }
        
        internal static class Regex
        {
            internal static string Extension
            {
                get { return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.Extension", @"^.*?[\w._-]+(\..+)$"); }
            }

            internal static string FileName
            {
                get { return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.FileName", @"^.*?([\w._-]+\..+)$"); }   
            }
        
            internal static string RelativePath
            {
                get { return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.RelativePath", @"^/-/assets/(.+(?:$extension)?)$"); }   
            }
        
            internal static string Timestamp
            {
                get { return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.Timestamp", @"^.+-(\d{10})?(?:$extension)?$"); }
            }   
        }
    }
}
