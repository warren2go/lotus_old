using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Sitecore.Diagnostics;
using Convert = System.Convert;

namespace Lotus.Foundation.Kernel.Extensions.RegularExpression
{
    public static class RegexExtensions
    {
        public static bool IsMatch(this string @string, string pattern)
        {
            return !string.IsNullOrEmpty(pattern) && Regex.Match(@string, pattern).Success;
        }
        
        [Sitecore.NotNull]
        public static string ReplacePattern(this string @string, string pattern, object replacement = null)
        {
            return Regex.Replace(@string, pattern, replacement != null ? replacement.ToString() : string.Empty);
        }
        
        [Sitecore.NotNull]
        public static string ExtractPattern(this string @string, string pattern, int index = 1, string @default = "")
        {
            return Regex.Match(@string, pattern).GetValueFromMatch(index, @default);
        }
        
        [Sitecore.NotNull]
        public static IEnumerable<string> ExtractPatterns(this string @string, string pattern)
        {
            return Regex.Matches(@string, pattern).GetValuesFromMatch();
        }
        
        [Sitecore.NotNull]
        public static IDictionary<int, string> ExtractPatternsWithIndexes(this string @string, string pattern)
        {
            return Regex.Matches(@string, pattern).GetValuesFromMatchWithIndexes();
        }
        
        [Sitecore.CanBeNull]
        public static T ExtractPattern<T>(this string @string, string pattern, int index = 1, T @default = default(T))
        {
            try
            {
                var value = Regex.Match(@string, pattern).GetValueFromMatch(index);
                if (string.IsNullOrEmpty(value))
                    return @default;
                return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                Log.Error("Extracting pattern failed from string = {0} with {1} at index {2}".FormatWith(@string, pattern, index), exception, typeof(RegexExtensions));
                #if DEBUG
                throw;
                #else
                return @default;
                #endif
            }
        }
        
        [Sitecore.NotNull]
        public static string GetValueFromMatch(this Match match, int index = 1, string @default = "")
        {
            if (index < 0)
            {
                index = match.Groups.Count - index;
            }
            return match.Success && index < match.Groups.Count ? match.Groups[index].Value : @default;
        }
        
        [Sitecore.NotNull]
        public static IEnumerable<string> GetValuesFromMatch(this MatchCollection matches)
        {
            var values = new List<string>();
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        var value = match.Groups[i].Value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            values.Add(value);   
                        }
                    }
                }   
            }
            return values;
        }
        
        [Sitecore.NotNull]
        internal static IDictionary<int, string> GetValuesFromMatchWithIndexes(this MatchCollection matches)
        {
            var values = new Dictionary<int, string>();
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        var value = match.Groups[i].Value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            values.Add(i, value);
                        }
                    }
                }
            }
            return values;
        }
    }
}