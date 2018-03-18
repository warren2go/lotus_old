using System.Collections.Specialized;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class NameValueListFieldExtensions
    {
        [NotNull]
        public static NameValueListField GetNameValueList(this Item item, [NotNull] string fieldName)
        {
            return item.Fields[fieldName];
        }

        [NotNull]
        public static NameValueCollection GetNameValueListCollection(this Item item, [NotNull] string fieldName)
        {
            if (item.Fields[fieldName] == null)
                return new NameValueCollection();
            return item.Fields[fieldName].GetNameValueListCollection();
        }

        [NotNull]
        public static NameValueCollection GetNameValueListCollection(this Field field)
        {
            var nameValueListField = (NameValueListField) field;
            return nameValueListField == null ? new NameValueCollection() : nameValueListField.NameValues;
        }
    }
}