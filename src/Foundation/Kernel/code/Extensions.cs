using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Diagnostics;
using Convert = Sitecore.Convert;

namespace Lotus.Foundation.Kernel
{
    internal static class Extensions
    {
        internal static readonly string[] Escapable = { @".|\.", @"+|\+", @"*|\*", @"^|\^", @"?|\?",  @"$|\$", @"&|&amp;", @"-|\-", @"(|\(", @")|\)" };
        
        internal static string Escape(this string @string, params string[] ignore)
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
        
        internal static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        [StringFormatMethod("format")]
        internal static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }
        
        internal static string Join<T>(this string joinWith, [NotNull] IEnumerable<T> list)
        {
            Assert.IsNotNull(list, "{0} is required.", nameof(list));

            using (var enumerator = list.GetEnumerator())
            {
                var sb = new StringBuilder();

                if (!enumerator.MoveNext())
                    return string.Empty;

                while (true)
                {
                    sb.Append(enumerator.Current);
                    if (!enumerator.MoveNext())
                        break;

                    sb.Append(joinWith);
                }

                if (sb.Length > 0)
                    sb.Remove(sb.Length - joinWith.Length, joinWith.Length);

                return sb.ToString();   
            }
        }
        
        internal static bool IsMatch(this string @string, string pattern)
        {
            return !string.IsNullOrEmpty(pattern) && Regex.Match(@string, pattern).Success;
        }
        
        internal static string ReplacePattern(this string @string, string pattern, object replacement = null)
        {
            return Regex.Replace(@string, pattern, replacement != null ? replacement.ToString() : string.Empty);
        }
        
        internal static string ExtractPattern(this string @string, string pattern, int index = 1, string @default = "")
        {
            return Regex.Match(@string, pattern).GetValueFromMatch(index, @default);
        }
        
        internal static IEnumerable<string> ExtractPatterns(this string @string, string pattern)
        {
            return Regex.Matches(@string, pattern).GetValuesFromMatch();
        }
        
        internal static IDictionary<int, string> ExtractPatternsWithIndexes(this string @string, string pattern)
        {
            return Regex.Match(@string, pattern).GetValuesFromMatchWithIndexes();
        }
        
        internal static T ExtractPattern<T>(this string @string, string pattern, int index = 1, T @default = default(T))
        {
            try
            {
                var value = Regex.Match(@string, pattern).GetValueFromMatch(index);
                if (string.IsNullOrEmpty(value))
                    return @default;
                return (T) System.Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                #if DEBUG
                throw;
                #endif
                return @default;
            }
        }
        
        internal static string GetValueFromMatch(this Match match, int index = 1, string @default = "")
        {
            if (index < 0)
            {
                index = match.Groups.Count - index;
            }
            return match.Success && index < match.Groups.Count ? match.Groups[index].Value : @default;
        }
        
        internal static IEnumerable<string> GetValuesFromMatch(this MatchCollection matches)
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
        
        internal static IDictionary<int, string> GetValuesFromMatchWithIndexes(this Match match)
        {
            var values = new Dictionary<int, string>();
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
            return values;
        }
    }
}