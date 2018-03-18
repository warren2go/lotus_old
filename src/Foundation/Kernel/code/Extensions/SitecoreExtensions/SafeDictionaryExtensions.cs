using Sitecore.Collections;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class SafeDictionaryExtensions
    {
        [CanBeNull]
        public static TValue TryGetValueOrDefault<TKey, TValue>(this SafeDictionary<TKey, TValue> dictionary, [NotNull] TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        [CanBeNull]
        public static TKeyAndValue TryGetValueOrDefault<TKeyAndValue>(this SafeDictionary<TKeyAndValue> dictionary, [NotNull] TKeyAndValue key, TKeyAndValue @default = default(TKeyAndValue))
        {
            TKeyAndValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
    }
}