using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets
{
    public static class Extensions
    {
        public static void RedirectBad(this HttpContext context, string url)
        {
            context.RedirectIgnored(url);
        }

        public static void RedirectIgnored(this HttpContext context, string url)
        {
            if (AssetsSettings.IgnoreType.IsMatch("^querystring$"))
            {
                if (!url.Contains("?"))
                {
                    url += "?ignore=true";
                }
                else
                {
                    url += "&ignore=true";
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
            context.RedirectPermanent(url);
        }
        
        public static void RedirectWithUpdate(this HttpContext context, int timestamp, string relativePath, string extension)
        {
            context.RedirectPermanent("~/-/assets/{0}".FormatWith(relativePath.ReplacePattern(extension.Escape(), "-{0:0000000000}{1}".FormatWith(timestamp, extension))));
        }
    }
}