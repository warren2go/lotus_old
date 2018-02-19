using System.Collections.Generic;
using System.Text;
using Sitecore;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel
{
    internal static class Extensions
    {
        internal static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default = default(TValue))
        {
            TValue exists;
            return dictionary.TryGetValue(key, out exists) ? exists : @default;
        }
        
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
    }
}