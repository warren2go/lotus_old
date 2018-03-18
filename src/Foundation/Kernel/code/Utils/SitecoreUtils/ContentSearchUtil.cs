using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore;
using Sitecore.ContentSearch.Linq;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class ContentSearchUtil
    {
        /// <summary>
        /// Search the index using an indexable item to resolve the index.
        /// </summary>
        /// <param name="indexable">The indexable item that also has information about the index.</param>
        /// <param name="func">The callback to invoke after resolving the index and queryable.</param>
        /// <typeparam name="TItem">A class that inhertis the SearchResultItem.</typeparam>
        /// <typeparam name="T">The resulting type after the search is complete.</typeparam>
        /// <returns>Determined by the callbacks result.</returns>
        [CanBeNull]
        public static T SearchIndex<TItem, T>([NotNull] IIndexable indexable, [NotNull] Func<IQueryable<TItem>, T> func)
        {
            var index = ContentSearchManager.GetIndex(indexable);
            if (index == null) return default(T);
            return SearchIndex(index, func);
        }
        
        /// <summary>
        /// Search the index using an indexable item to resolve the index.
        /// </summary>
        /// <param name="name">The name of the index.</param>
        /// <param name="func">The callback to invoke after resolving the index and queryable.</param>
        /// <typeparam name="TItem">A class that inhertis the SearchResultItem.</typeparam>
        /// <typeparam name="T">The resulting type after the search is complete.</typeparam>
        /// <returns>Determined by the callbacks result.</returns>
        [CanBeNull]
        public static T SearchIndex<TItem, T>(string name, [NotNull] Func<IQueryable<TItem>, T> func)
        {
            var index = ContentSearchManager.GetIndex(name);
            if (index == null) return default(T);
            return SearchIndex(index.CreateSearchContext(), func);
        }
        
        /// <summary>
        /// Search the index using an indexable item to resolve the index.
        /// </summary>
        /// <param name="index">An instance of the index.</param>
        /// <param name="func">The callback to invoke after resolving the index and queryable.</param>
        /// <typeparam name="TItem">A class that inhertis the SearchResultItem.</typeparam>
        /// <typeparam name="T">The resulting type after the search is complete.</typeparam>
        /// <returns>Determined by the callbacks result.</returns>
        [CanBeNull]
        public static T SearchIndex<TItem, T>([NotNull] ISearchIndex index, [NotNull] Func<IQueryable<TItem>, T> func)
        {
            return SearchIndex(index.CreateSearchContext(), func);
        }

        /// <summary>
        /// Search the index using an indexable item to resolve the index.
        /// </summary>
        /// <param name="context">An instance of the search context.</param>
        /// <param name="func">The callback to invoke after resolving the index and queryable.</param>
        /// <typeparam name="TItem">A class that inhertis the SearchResultItem.</typeparam>
        /// <typeparam name="T">The resulting type after the search is complete.</typeparam>
        /// <returns>Determined by the callbacks result.</returns>
        [CanBeNull]
        public static T SearchIndex<TItem, T>([NotNull] IProviderSearchContext context, [NotNull] Func<IQueryable<TItem>, T> func)
        {
            return func.Invoke(context.GetQueryable<TItem>());
        }

        /// <summary>
        /// Get the results from the queryable
        /// </summary>
        public static SearchResults<TItem> GetResults<TItem>([NotNull] IQueryable<TItem> queryable)
        {
            return queryable.GetResults();
        }
    }
}