using Sitecore.ContentSearch;
using Sitecore.Data.Items;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class ContentSearchExtensions
    {
        public static IIndexable AsIndexable(this Item item)
        {
            return (SitecoreIndexableItem) item;
        }
    }
}