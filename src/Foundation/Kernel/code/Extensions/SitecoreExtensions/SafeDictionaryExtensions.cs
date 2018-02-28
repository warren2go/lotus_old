using Sitecore.Collections;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class SafeDictionaryExtensions
    {
        public static TValue TryGetValueOrDefault<TKey, TValue>(this SafeDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
        public static TKeyAndValue TryGetValueOrDefault<TKeyAndValue>(this SafeDictionary<TKeyAndValue> dictionary, TKeyAndValue key, TKeyAndValue @default = default(TKeyAndValue))
        {
            TKeyAndValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
    }
}