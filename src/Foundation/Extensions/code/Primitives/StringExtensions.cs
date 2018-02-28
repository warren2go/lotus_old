using System;
using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.RegularExpression;
using Sitecore;

namespace Lotus.Foundation.Extensions.Primitives
{
    public static class StringExtensions
    {
        private static readonly string[] Escapable = { @".|\.", @"+|\+", @"*|\*", @"^|\^", @"?|\?",  @"$|\$", @"&|&amp;", @"-|\-", @"(|\(", @")|\)" };
        
        public static string SurroundsWith(this string @string, string with)
        {
            return string.Format("{0}{1}{2}",with, @string, with);
        }

        [StringFormatMethod("format")]
        public static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

        public static string Escape(this string @string, params string[] ignore)
        {
            foreach (var escape in Escapable)
            {
                var seek = escape.Split('|').FirstOrDefault() ?? string.Empty;
                if (ignore.Any(x => x.Equals(seek, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                if (!string.IsNullOrEmpty(seek))
                {
                    @string = @string.Replace(seek, escape.Split('|').LastOrDefault() ?? seek);   
                }
            }
            return @string;
        }
        
        public static string WhenEmpty(this string @string, string fallback)
        {
            return string.IsNullOrEmpty(@string) ? fallback : @string;
        }
        
        public static string WhenEmpty(this string @string, Func<string, string> invoke)
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
    }
}