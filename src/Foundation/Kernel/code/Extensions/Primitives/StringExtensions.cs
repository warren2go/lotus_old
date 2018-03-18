using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Utils;
using Sitecore;
using StringUtil = Lotus.Foundation.Kernel.Utils.StringUtil;

namespace Lotus.Foundation.Kernel.Extensions.Primitives
{
    public static class StringExtensions
    {
        private static readonly string[] Escapable = { @".|\.", @"+|\+", @"*|\*", @"^|\^", @"?|\?",  @"$|\$", @"&|&amp;", @"-|\-", @"(|\(", @")|\)" };
        
        [NotNull]
        public static string SurroundsWith(this string @string, string with)
        {
            return string.Format("{0}{1}{2}", with, @string, with);
        }

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

        [NotNull]
        public static string Escape(this string @string, params string[] ignore)
        {
            foreach (var escape in Escapable)
            {
                var seek = escape.Split('|').FirstOrDefault() ?? string.Empty;
                if (seek.ContainsRegex(ignore, RegexOptions.IgnoreCase))
                    continue;
                if (!string.IsNullOrEmpty(seek))
                {
                    @string = @string.Replace(seek, escape.Split('|').LastOrDefault() ?? seek);   
                }
            }
            return @string;
        }
        
        [CanBeNull]
        public static string WhenEmpty(this string @string, [CanBeNull] string whenEmpty)
        {
            return string.IsNullOrEmpty(@string) ? whenEmpty : @string;
        }
        
        [CanBeNull]
        public static string WhenEmpty(this string @string, [NotNull] Func<string, string> invoke)
        {
            return string.IsNullOrEmpty(@string) ? invoke.Invoke(@string) : @string;
        }
        
        public static bool ToBool(this string @string, bool @default = default(bool))
        {
            if (@string == "1")
                return true;
            if (@string == "0")
                return false;
            bool b;
            return bool.TryParse(@string, out b) ? b : @default;
        }

        public static bool ContainsRegex(this string @string, [NotNull] string[] expressions, params RegexOptions[] regexOptions)
        {
            return StringUtil.ContainsRegex(@string, expressions, regexOptions);
        }
    }
}