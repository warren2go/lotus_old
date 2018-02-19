using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Lotus.Foundation.Extensions.SitecoreExtensions
{
    public static class LinkFieldExtensions
    {
        public static LinkField AsLinkField(this Field field)
        {
            return field;
        }
        
        public static LinkField GetLinkField(this Item item, string linkFieldName)
        {
            if (string.IsNullOrEmpty(linkFieldName) || item.Fields[linkFieldName] == null)
                return default(LinkField);
            return item.Fields[linkFieldName].AsLinkField();
        }
        
        
        public static string GetLinkFieldUrl(this Item item, string linkFieldName)
        {
            if (string.IsNullOrEmpty(linkFieldName))
                return string.Empty;
            
            return WebUtil.GetUrl(item, linkFieldName);
        }

        public static Item GetLinkFieldTargetItem(this Item item, string linkFieldName)
        {
            if (item == null || item.Fields[linkFieldName] == null)
                return null;
            return item.Fields[linkFieldName].GetLinkFieldTargetItem();
        }

        public static string GetLinkFieldValue(this Item item, string linkFieldName)
        {
            if (item.Fields[linkFieldName] == null)
                return null;
            return item.Fields[linkFieldName].GetLinkFieldValue();
        }

        public static Item GetLinkFieldTargetItem(this Field field)
        {
            return ((LinkField)field).TargetItem;
        }

        public static string GetLinkFieldValue(this Field field)
        {
            return ((LinkField)field).Value;
        }

        public static string GetLinkFieldText(this Item item, string linkFieldName)
        {
            if (item.Fields[linkFieldName] == null)
                return null;
            return item.Fields[linkFieldName].GetLinkFieldText();
        }

        public static string GetLinkFieldText(this Field field)
        {
            return ((LinkField)field).Text;
        }
    }
}