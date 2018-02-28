﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static string Join<T>(this IEnumerable<T> collection, string delimiter = ", ", string pattern = "{0}")
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
                        sb.Append(string.Format(pattern, item));   
                    }
                    sb.Append(delimiter);
                }
            }
            var result = sb.ToString();
            return result.Substring(0, result.Length - delimiter.Length);
        }
        
        public static string JoinWithInvoke<T>(this IEnumerable<T> collection, Func<T, string> invoke, string delimiter = ", ", string pattern = "{0}")
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
                        if (invoke != null)
                        {
                            sb.Append(string.Format(pattern, invoke.Invoke(item)));
                        }
                        else
                        {
                            sb.Append(string.Format(pattern, item));   
                        }   
                    }
                    sb.Append(delimiter);
                }
            }
            var result = sb.ToString();
            return result.Substring(0, result.Length - delimiter.Length);
        }
        
        [NotNull]
        [Obsolete("Use collection.Join(delimter, pattern) instead.")]
        public static string Dump<T>(this IEnumerable<T> collection, string delimiter = ", ", string pattern = "{0}")
        {
            return Join(collection, delimiter, pattern);
        }

        [NotNull]
        [Obsolete("Use collection.JoinWithInvoke(invoke, delimter, pattern) instead.")]
        public static string DumpWithInvoke<T>(this IEnumerable<T> collection, [NotNull] Func<T, string> invoke, string delimiter = ", ", string pattern = "{0}")
        {
            return JoinWithInvoke(collection, invoke, delimiter, pattern);
        }
        
        public static void For<T>(this IEnumerable<T> collection, Action<T> action, bool reversed = true)
        {
            var array = collection as T[];
            if (array != null)
            {
                var count = array.Length;
                if (reversed)
                {
                    for (var i = count; i > 0; i--)
                    {
                        action.Invoke(array[i + 1]);
                    }   
                }
                else
                {
                    for (var i = 0; i < count; i++)
                    {
                        action.Invoke(array[i]);
                    }
                }
            }
        }
        
        public static IEnumerable<T> ForAnd<T>(this IEnumerable<T> collection, Action<T> action, bool reversed = true)
        {
            var array = collection as T[];
            if (array != null)
            {
                var count = array.Length;
                if (reversed)
                {
                    for (var i = count; i > 0; i--)
                    {
                        action.Invoke(array[i + 1]);
                    }   
                }
                else
                {
                    for (var i = 0; i < count; i++)
                    {
                        action.Invoke(array[i]);
                    }
                }
            }
            return collection;
        }
    }
}