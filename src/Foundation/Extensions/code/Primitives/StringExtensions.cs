﻿using System;
using System.Linq;
using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Foundation.Extensions.Primitives
{
    public static class StringExtensions
    {
        private static readonly string[] Escapable = { @".|\.", @"+|\+", @"*|\*", @"^|\^", @"?|\?",  @"$|\$", @"&|&amp;", @"-|\-" };
        
        public static string SurroundsWith(this string @string, string with)
        {
            return string.Format("{0}{1}{2}",with, @string, with);
        }

        public static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

        public static string Escape(this string @string)
        {
            foreach (var escape in Escapable)
            {
                var seek = escape.Split('|').FirstOrDefault();
                var replace = escape.Split('|').LastOrDefault();
                if (!string.IsNullOrEmpty(seek) && !string.IsNullOrEmpty(replace))
                {
                    @string = @string.Replace(seek, replace);   
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