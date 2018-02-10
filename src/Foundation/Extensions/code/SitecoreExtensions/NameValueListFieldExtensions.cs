using System.Collections.Specialized;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Lotus.Foundation.Extensions.SitecoreExtensions
{
    public static class NameValueListFieldExtensions
    {
        public static NameValueListField GetNameValueList(this Item item, string fieldName)
        {
            return item.Fields[fieldName];
        }

        public static NameValueCollection GetNameValueListCollection(this Item item, string fieldName)
        {
            if (item.Fields[fieldName] == null)
                return new NameValueCollection();
            return item.Fields[fieldName].GetNameValueListCollection();
        }

        public static NameValueCollection GetNameValueListCollection(this Field field)
        {
            var nameValueListField = (NameValueListField) field;
            return nameValueListField == null ? new NameValueCollection() : nameValueListField.NameValues;
        }
    }
}