using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Drawing.Exif.Attributes;

namespace Lotus.Foundation.Logging
{
    internal static class Extensions
    {
        internal static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }

        internal static TValue TryGetValueOrDefault<TKey, TValue>(this SafeDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        internal static TKeyAndValue TryGetValueOrDefault<TKeyAndValue>(this SafeDictionary<TKeyAndValue> dictionary, TKeyAndValue key, TKeyAndValue @default = default(TKeyAndValue))
        {
            TKeyAndValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        [StringFormatMethod("format")]
        internal static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

        internal static IEnumerable<T> EachAnd<T>(this IEnumerable<T> items, Func<T, T> func)
        {
            var enumerable = items as T[] ?? items.ToArray();
            for (var i = 0; i < enumerable.Length; i++)
            {
                enumerable[i] = func.Invoke(enumerable.ElementAtOrDefault(i));
            }
            return enumerable;
        }
        
        internal static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            var enumerable = items as T[] ?? items.ToArray();
            for (var i = 0; i < enumerable.Length; i++)
            {
                action.Invoke(enumerable.ElementAtOrDefault(i));
            }
        }

        internal static bool ToBool(this string @string, bool @default = default(bool))
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