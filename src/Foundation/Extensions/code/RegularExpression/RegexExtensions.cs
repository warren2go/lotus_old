using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lotus.Foundation.Extensions.RegularExpression
{
    public static class RegexExtensions
    {
        public static bool IsMatch(this string @string, string pattern)
        {
            return !string.IsNullOrEmpty(pattern) && Regex.Match(@string, pattern).Success;
        }
        
        public static string ReplacePattern(this string @string, string pattern, object replacement = null)
        {
            return Regex.Replace(@string, pattern, replacement != null ? replacement.ToString() : string.Empty);
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