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

        internal static bool Strict
        {
            get
            {
                return Sitecore.Configuration.Settings.GetBoolSetting("Lotus.Foundation.Assets.Strict", false);
            }
        }

        internal static string IgnoreType
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.IgnoreType", "querystring");
            }
        }

        internal static string NotFoundUrl
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.NotFoundUrl", string.Empty);
            }
        }

        internal static class CDN
        {
            internal static string Redirect
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.CDN.Redirect", string.Empty);
                }
            }   
        }

        internal static class Caching
        {
            internal static int DefaultExpireHours
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetIntSetting("Lotus.Foundation.Assets.Caching.DefaultExpireHours", 0);
                }
            }
        }
        
        internal static class Regex
        {
            internal static string Extension
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.Extension", @"^.*?[\w._-]+(\..+)$");
                }
            }

            internal static string FileName
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.FileName", @"^.*?([\w._-]+\..+)$");
                }   
            }
        
            internal static string RelativePath
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.RelativePath", @"^/-/assets/(.+(?:$extension)?)$");
                }   
            }
        
            internal static string Timestamp
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.Timestamp", @"^.+-(\d{10})?(?:$extension)?$");
                }
            }

            internal static string ParentPath
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.ParentPath", ".*");
                    
                }
            }

            internal static string IgnoreQuery
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.IgnoreQuery", @"([?&amp;]?ignore=\w+)");
                }
            }

            internal static string BlockedQuery
            {
                get
                {
                    //built from: https://www.iis.net/downloads/community/2011/05/dynamic-image-resizing-module-v3
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Regex.BlockedQuery",
                        @"[?&amp;]?((?:w|width|h|height|mw|maxwidth|mh|maxheight|rotate|crop|bgcolor|stretch|scale|flip|sourceflip|paddingwidth|borderwidth|format|colors|frame|page)=\w+)");
                }
            }
        }

        internal static class Compression
        {
            internal static string Supported
            {
                get
                {
                    return Sitecore.Configuration.Settings.GetSetting("Lotus.Foundation.Assets.Compression.Supported", string.Empty);
                }
            }
        }
    }
}
