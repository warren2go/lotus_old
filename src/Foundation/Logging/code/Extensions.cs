using System.Collections.Generic;

namespace Lotus.Foundation.Logging
{
    internal static class Extensions
    {
        internal static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
    }
}