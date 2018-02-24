using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;
using Sitecore;

namespace Lotus.Foundation.Assets
{
    internal static class Extensions
    {
        [TerminatesProgram]
        internal static void RedirectBad(this HttpContextBase context, string url)
        {
            context.RedirectIgnored(url);
        }

        [TerminatesProgram]
        internal static void RedirectIgnored(this HttpContextBase context, string url)
        {
            var existingQuery = context.Request.Url.Query;
            if (Settings.IgnoreType.IsMatch("^querystring$"))
            {
                if (string.IsNullOrEmpty(existingQuery))
                {
                    existingQuery += "?ignore=true";
                }
                else
                {
                    var ignore = existingQuery.ExtractPattern(Settings.Regex.IgnoreQuery);
                    if (string.IsNullOrEmpty(ignore))
                    {
                        existingQuery += "&ignore=true";
                    }
                    else
                    {
                        existingQuery = existingQuery.Replace(ignore, "&ignore=true");
                    }
                }
            }
            if (Settings.IgnoreType.IsMatch("^timestamp$"))
            {
                if (url.IsMatch(Settings.Regex.Timestamp))
                {
                    url = url.ReplacePattern(Settings.Regex.Timestamp, "0000000000");
                }
                else
                {
                    var extension = url.ExtractPattern(Settings.Regex.Extension);
                    url = url.ReplacePattern(extension.Escape(), "-{0}{1}".FormatWith("0000000000", extension));
                }
            }
            context.RedirectPermanent(url + existingQuery);
        }
        
        [TerminatesProgram]
        internal static void RedirectWithUpdate(this HttpContextBase context, int timestamp, string relativePath, string extension)
        {
            var url = "~/-/assets/{0}".FormatWith(relativePath.ReplacePattern(extension.Escape(),
                "-{0:0000000000}{1}".FormatWith(timestamp, extension)));
            var query = context.Request.Url.Query;
            context.RedirectPermanent(url + query);
        }
    }
}