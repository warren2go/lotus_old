using System;
using Lotus.Foundation.Kernel.Extensions.SitecoreExtensions;
using Lotus.Foundation.Kernel.Utils.SitecoreUtils;
using NUnit.Framework;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Lotus.Foundation.Kernel.Tests.Examples
{
    public class ContentSearchExamples
    {
        /// <summary>
        /// Example snippet for using the <see cref="ContentSearchUtil.SearchIndex" /> with an index resolved by name.
        /// </summary>
        public void ContentSearch_SearchIndexExample1()
        {
            ContentSearchUtil.SearchIndex<SearchResultItem, object>("sitecore_master", queryable =>
            {
                var results = ContentSearchUtil.GetResults(queryable);
                return null;
            });
        }
        
        /// <summary>
        /// Example snippet for using the <see cref="ContentSearchUtil.SearchIndex" /> with an index resolved by instance.
        /// </summary>
        public void ContentSearch_SearchIndexExample2()
        {
            var index = ContentSearchManager.GetIndex("sitecore_master");
            ContentSearchUtil.SearchIndex<SearchResultItem, object>(index, queryable =>
            {
                var results = ContentSearchUtil.GetResults(queryable);
                return null;
            });
        }
        
        /// <summary>
        /// Example snippet for using the <see cref="ContentSearchUtil.SearchIndex" /> with an index resolved by an item.
        /// </summary>
        public void ContentSearch_SearchIndexExample3()
        {
            var item = Sitecore.Context.Database.GetItem("/sitecore/content/Home");
            ContentSearchUtil.SearchIndex<SearchResultItem, object>(item.AsIndexable(), queryable =>
            {
                var results = ContentSearchUtil.GetResults(queryable);
                return null;
            });
        }
        
        /// <summary>
        /// Example snippet for using the <see cref="ContentSearchUtil.SearchIndex" /> with an index resolved by a search context.
        /// </summary>
        public void ContentSearch_SearchIndexExample4()
        {
            var index = ContentSearchManager.GetIndex("sitecore_master");
            ContentSearchUtil.SearchIndex<SearchResultItem, object>(index.CreateSearchContext(), queryable =>
            {
                var results = ContentSearchUtil.GetResults(queryable);
                return null;
            });
        }
    }
}