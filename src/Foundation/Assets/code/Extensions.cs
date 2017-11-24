using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets
{
    public static class Extensions
    {
        public static void RedirectBad(this HttpContextBase context, string url)
        {
            context.RedirectIgnored(url);
        }

        public static void RedirectIgnored(this HttpContextBase context, string url)
        {
            var existingQuery = context.Request.Url.Query;
            if (AssetsSettings.IgnoreType.IsMatch("^querystring$"))
            {
                if (string.IsNullOrEmpty(existingQuery))
                {
                    existingQuery += "?ignore=true";
                }
                else
                {
                    var ignore = existingQuery.ExtractPattern(AssetsSettings.Regex.IgnoreQuery);
                    if (string.IsNullOrEmpty(ignore))
                    {
                        existingQuery += "&ignore=true";
                    }
                    else
                    {
                        existingQuery.Replace(ignore, "&ignore=true");
                    }
                }
            }
            if (AssetsSettings.IgnoreType.IsMatch("^timestamp$"))
            {
                if (url.IsMatch(AssetsSettings.Regex.Timestamp))
                {
                    url = url.ReplacePattern(AssetsSettings.Regex.Timestamp, "0000000000");
                }
                else
                {
                    var extension = url.ExtractPattern(AssetsSettings.Regex.Extension);
                    url = url.ReplacePattern(extension.Escape(), "-{0}{1}".FormatWith("0000000000", extension));
                }
            }
            context.RedirectPermanent(url + existingQuery);
        }
        
        public static void RedirectWithUpdate(this HttpContextBase context, int timestamp, string relativePath, string extension)
        {
            var url = "~/-/assets/{0}".FormatWith(relativePath.ReplacePattern(extension.Escape(),
                "-{0:0000000000}{1}".FormatWith(timestamp, extension)));
            var query = context.Request.Url.Query;
            context.RedirectPermanent(url + query);
        }
    }
}