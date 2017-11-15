using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lotus.Foundation.Extensions
{
    public static class Extensions
    {
        private static readonly string[] Escapable = { @".", @"+", @"*", @"^", @"?",  @"$" };
    
        private static readonly DateTime StaticDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static TValue TryGetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            var exists = default(TValue);
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        public static int ToUnixTimestamp(this DateTime date, bool universal = true)
        {
            return (int)((universal ? date : date.ToUniversalTime()) - StaticDateTime).TotalSeconds;
        }

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
                @string = @string.Replace(escape, @"\{0}".FormatWith(escape));
            }
            return @string;
        }

        public static bool IsMatch(this string @string, string pattern)
        {
            return Regex.Match(@string, pattern).Success;
        }
        
        public static string ReplacePattern(this string @string, string pattern, string replacement = "")
        {
            return Regex.Replace(@string, pattern, replacement);
        }
        
        public static string ExtractPattern(this string @string, string pattern, int index = 1, string @default = "")
        {
            return Regex.Match(@string, pattern).GetValueFromMatch(index, @default);
        }
        
        public static T ExtractPattern<T>(this string @string, string pattern, int index = 1, string @default = "")
        {
            try
            {
                var value = Regex.Match(@string, pattern).GetValueFromMatch(index, @default);
                if (string.IsNullOrEmpty(value))
                    return default(T);
                return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                #if DEBUG
                throw;
                #endif
                return default(T);
            }
        }
        
        public static string GetValueFromMatch(this Match match, int index = 1, string @default = "")
        {
            if (index < 0)
            {
                index = match.Groups.Count - index;
            }
            return match.Success && index < match.Groups.Count ? match.Groups[index].Value : @default;
        }
    }
}