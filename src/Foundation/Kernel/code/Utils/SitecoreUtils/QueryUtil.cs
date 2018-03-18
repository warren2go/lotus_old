using System.Linq;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Sitecore;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class QueryUtil
    {
        public static readonly string[] SanitizeFastQueryReplacements = { @"(?<prefix>[a-zA-Z0-9/]+)(?<sanitize>[\-\s]+)(?<postfix>[a-zA-Z0-9/]+[\*/])|${prefix}#${sanitize}#${postfix}" };
        public static readonly string[] SanitizeXPathRegex = { @"" };
        public static readonly string[] SanitizeQueryRegex = { @"" };
        
        /// <summary>
        /// Sanitize a fast query, replacing illegal characters
        /// </summary>
        public static string SanitizeFastQuery([NotNull] string query)
        {
            Assert.ArgumentNotNullOrEmpty(query, nameof(query));
            return SanitizeWithRegexPairs(query, SanitizeFastQueryReplacements);
        }

        /// <summary>
        /// Sanitize an xpath query, replacing illegal characters
        /// </summary>
        public static string SanitizeXPathQuery([NotNull] string query)
        {
            Assert.ArgumentNotNull(query, nameof(query));
            return SanitizeWithRegexPairs(query, SanitizeXPathRegex);
        }

        /// <summary>
        /// Sanitize standard Sitecore queries, replacing illegal characters
        /// </summary>
        public static string SanitizeQuery([NotNull] string query)
        {
            Assert.ArgumentNotNullOrEmpty(query, nameof(query));
            if (string.IsNullOrEmpty(query)) return string.Empty;
            if (query.StartsWith("fast:"))
                return SanitizeFastQuery(query);
            if (query.StartsWith("search:"))
                return SanitizeXPathQuery(query);
            return SanitizeWithRegexPairs(query, SanitizeQueryRegex);
        }

        private static string SanitizeWithRegexPairs(string query, string[] sanitizations)
        {
            foreach (var sanitize in sanitizations)
            {
                var regexSeek = Sitecore.StringUtil.GetPrefix(sanitize, '|');
                var regexSubstitute = Sitecore.StringUtil.GetPostfix(sanitize, '|');
                query = query.ReplacePattern(regexSeek, regexSubstitute);
            }
            return query;
        }
    }
}