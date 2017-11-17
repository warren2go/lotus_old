using System.Collections.Generic;

namespace Lotus.Foundation.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            var exists = default(TValue);
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
    }
}