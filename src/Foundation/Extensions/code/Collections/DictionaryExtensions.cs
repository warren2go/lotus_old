using System.Collections.Generic;
using System.Text;
using Lotus.Foundation.Extensions.Primitives;

namespace Lotus.Foundation.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            var exists = default(TValue);
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }

        public static string Dump<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string delimiter = ",", string pattern = "{0}={1}")
        {
            var sb = new StringBuilder("{0}:KeyValuePair<{1},{2}>".FormatWith(nameof(dictionary), typeof(TKey), typeof(TValue)));
            foreach (var keyValuePair in dictionary)
            {
                sb.Append(pattern.FormatWith(keyValuePair.Key, keyValuePair.Value) + delimiter);
            }
            var result = sb.ToString();
            return result.Substring(0, result.Length - delimiter.Length);
        }
    }
}