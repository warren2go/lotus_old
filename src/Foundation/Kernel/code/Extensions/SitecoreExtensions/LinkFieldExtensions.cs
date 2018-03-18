using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class LinkFieldExtensions
    {
        [NotNull]
        public static LinkField AsLinkField(this Field field)
        {
            return field;
        }
        
        [NotNull]
        public static LinkField GetLinkField(this Item item, string linkFieldName)
        {
            if (string.IsNullOrEmpty(linkFieldName) || item.Fields[linkFieldName] == null)
                return default(LinkField);
            return item.Fields[linkFieldName].AsLinkField();
        }
        
        [NotNull]
        public static string GetLinkFieldUrl(this Item item, string linkFieldName)
        {
            if (string.IsNullOrEmpty(linkFieldName))
                return string.Empty;
            return WebUtil.GetUrl(item, linkFieldName);
        }

        [CanBeNull]
        public static Item GetLinkFieldTargetItem(this Item item, string linkFieldName)
        {
            if (item == null || item.Fields[linkFieldName] == null)
                return null;
            return item.Fields[linkFieldName].GetLinkFieldTargetItem();
        }

        [NotNull]
        public static string GetLinkFieldValue(this Item item, string linkFieldName)
        {
            if (item.Fields[linkFieldName] == null)
                return string.Empty;
            return item.Fields[linkFieldName].GetLinkFieldValue();
        }

        [CanBeNull]
        public static Item GetLinkFieldTargetItem(this Field field)
        {
            return ((LinkField)field).TargetItem;
        }

        [NotNull]
        public static string GetLinkFieldValue(this Field field)
        {
            return ((LinkField)field).Value;
        }

        [NotNull]
        public static string GetLinkFieldText(this Item item, string linkFieldName)
        {
            if (item.Fields[linkFieldName] == null)
                return string.Empty;
            return item.Fields[linkFieldName].GetLinkFieldText();
        }

        [NotNull]
        public static string GetLinkFieldText(this Field field)
        {
            return ((LinkField)field).Text;
        }
    }
}