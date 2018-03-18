using System.Collections.Generic;
using System.Text;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        [CanBeNull]
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, [NotNull] TKey key, [CanBeNull] TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }

        [NotNull]
        public static string Dump<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, [NotNull] string delimiter = ",", [NotNull] string pattern = "{0}={1}")
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