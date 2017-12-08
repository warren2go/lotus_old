using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lotus.Foundation.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static string Dump<T>(this IEnumerable<T> collection, string delimiter = ",")
        {
            var array = collection as T[];
            var sb = new StringBuilder(nameof(collection) + ":" + typeof(T));
            if (array != null)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    var item = array[i];
                    if (item != null)
                    {
                        sb.Append(item.ToString() + delimiter);   
                    }
                }
            }
            var result = sb.ToString();
            return result.Substring(0, result.Length - delimiter.Length);
        }
    }
}