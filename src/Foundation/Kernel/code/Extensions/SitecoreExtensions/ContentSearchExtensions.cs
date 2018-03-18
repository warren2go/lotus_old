using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class ContentSearchExtensions
    {
        /// <summary>
        /// Cast the item into a SitecoreIndexableItem which implements IIndexable
        /// </summary>
        [NotNull]
        public static IIndexable AsIndexable(this Item item)
        {
            return (SitecoreIndexableItem) item;
        }
    }
}