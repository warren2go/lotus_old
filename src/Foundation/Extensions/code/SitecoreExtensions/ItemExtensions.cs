using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Primitives;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Lotus.Foundation.Extensions.SitecoreExtensions
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Check whether an item has a template from the specified collection.
        /// </summary>
        public static bool HasTemplate(this Item item, params ID[] templateIds)
        {
            return templateIds.Any(x => x == item.TemplateID);
        }
        
        /// <summary>
        /// Check whether an item has a template from the specified collection.
        /// </summary>
        public static bool HasTemplate(this Item item, params string[] templateIds)
        {
            return templateIds.Any(x => x.Equals(item.TemplateID.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
        
        /// <summary>
        /// Get the item directly from the field via a lookup
        /// </summary>
        public static Item GetItemFromLookup(this Item item, string fieldName)
        {
            if (item.Fields[fieldName] == null)
            {
                Log.Error("Error getting item from field - item or field is null [{0}[{1}]".FormatWith(item.Paths.FullPath, fieldName), typeof(ItemExtensions));
                return null;
            }
            return item.Fields[fieldName].GetItemFromLookup();
        }

        /// <summary>
        /// Get the item directly from the field via a lookup
        /// </summary>
        public static Item GetItemFromLookup(this Field field)
        {
            try
            {
                var lookup = (LookupField)field;
                return lookup.TargetItem;
            }
            catch (Exception exception)
            {
                Log.Error("Error getting item from field - lookup failed [{0}]".FormatWith(field.Name), exception, typeof(ItemExtensions));
                return null;
            }
        }

        /// <summary>
        /// Get the items directly from the field via a multilist field.
        /// </summary>
        public static Item[] GetItemsFromMultilist(this Item item, string fieldName, bool allowNull = true)
        {
            if (item.Fields[fieldName] == null)
            {
                Log.Error("Error getting items from field - item or field is null [{0}[{1}]".FormatWith(item.Paths.FullPath, fieldName), typeof(ItemExtensions));
                return allowNull ? null : new Item[0];
            }
            return item.Fields[fieldName].GetItemsFromMultilist(allowNull);
        }

        /// <summary>
        /// Get the items directly from the field via a multilist field.
        /// </summary>
        public static Item[] GetItemsFromMultilist(this Field field, bool allowNull = true)
        {
            try
            {
                var multilist = (MultilistField)field;
                return multilist.GetItems();
            }
            catch (Exception exception)
            {
                Log.Error("Error getting items from field - lookup failed [{0}]".FormatWith(field.Name), exception, typeof(ItemExtensions));
                return allowNull ? null : new Item[0];
            }
        }

        public static IEnumerable<Item> GetDescendentsWithComparer(this Item item, Func<Item, bool> comparer)
        {
            return item.GetDescendents().Where(comparer);
        }

        public static IEnumerable<Item> GetDescendentsWithTemplate(this Item item, string templateid)
        {
            if (string.IsNullOrEmpty(templateid))
            {
                templateid = string.Empty;
            }

            var decendents = new List<Item>(item.Children.Where(x => templateid.Equals(x.TemplateID.ToString(), StringComparison.InvariantCultureIgnoreCase)));
            foreach (Item child in item.Children)
            {
                decendents.AddRange(child.GetDescendentsWithTemplate(templateid));
            }
            return decendents;
        }

        public static IEnumerable<Item> GetDescendents(this Item item, bool recursive = true)
        {
            if (recursive)
            {
                var decendents = new List<Item>(item.Children);
                foreach (Item child in item.Children)
                {
                    decendents.AddRange(child.GetDescendents());
                }
                return decendents;
            }
            else
            {
                return item.Children;
            }
        }
        
        public static string GetItemUrl(this Item item, UrlOptions urlOptions = null)
        {
            try
            {
                return urlOptions == null ? LinkManager.GetItemUrl(item) : LinkManager.GetItemUrl(item, urlOptions);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(ItemExtensions));
            }
            return string.Empty;
        }

        public static string GetItemUrlPrefixedSecure(this Item item, UrlOptions urlOptions, bool prefixProtocol, bool secure)
        {
            try
            {
                if (urlOptions == null)
                {
                    urlOptions = new UrlOptions();
                    urlOptions.AlwaysIncludeServerUrl = true;
                    urlOptions.LanguageEmbedding = LanguageEmbedding.Never;   
                }

                var itemUrl = item.GetItemUrl(urlOptions);

                if (prefixProtocol)
                {
                    if (!itemUrl.StartsWith("http") && !itemUrl.StartsWith(@"//") && !itemUrl.StartsWith(@"://"))
                    {
                        itemUrl = @"http://" + itemUrl;
                    }

                    if (itemUrl.StartsWith(@"//"))
                    {
                        itemUrl = @"http:" + itemUrl;
                    }

                    if (itemUrl.StartsWith(@"://"))
                    {
                        itemUrl = @"http" + itemUrl;
                    }
                }

                return secure ? itemUrl.Replace(@"http://", @"https://") : itemUrl;
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(ItemExtensions));
            }
            return string.Empty;
        }
        
        /// <summary>
        /// Get a value and cast from an items field via lookup
        /// </summary>
        public static T GetValueFromLookup<T>(this Item item, string fieldName, T @default = default(T))
        {
            if (item.Fields[fieldName] == null)
            {
#if DEBUG
                Log.Error("Error getting value from field - item or field is null", typeof(ItemExtensions));
#endif
                return @default;
            }
            return item.Fields[fieldName].GetValueFromLookup(@default);
        }

        /// <summary>
        /// Get a value and cast from a field via lookup
        /// </summary>
        public static T GetValueFromLookup<T>(this Field field, T @default = default(T))
        {
            try
            {
                var lookup = (LookupField)field;
                var value = lookup.Value;

                switch (field.Type.ToLower())
                {
                    case "checkbox":
                        value = value == "1" ? "True" : "False";
                        break;

                    case "date":
                    case "datetime":
                        if (typeof(T) == typeof(DateTime))
                        {
                            return DateUtil.IsoDateToDateTime(value, @default.CastTo<DateTime>()).CastTo<T>();
                        }
                        break;
                }

                return !string.IsNullOrEmpty(value) ? value.CastTo<T>() : @default;
            }
            catch (Exception exception)
            {
                Log.Error(string.Format("Error getting value from field - lookup failed [{0}->{1}]", field.Name, typeof(T).FullName), exception, typeof(ItemExtensions));
                return @default;
            }
        }

        public static string Dump(this Item item, bool dataOnly = false, string format = "{0}={1}", string delimiter = ", ")
        {
            var sb = new StringBuilder();

            var fields = item.Fields;

            foreach (Field field in fields)
            {
                if (dataOnly)
                {
                    sb.Append(field.GetValueFromLookup("-"));
                }
                else
                {
                    sb.Append(format.FormatWith(field.Name, field.GetValueFromLookup("-")));   
                }
                sb.Append(delimiter);
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - delimiter.Length, delimiter.Length);
            return sb.ToString();
        }

        /// <summary>
        /// Try to edit the fields value
        /// </summary>
        /// <returns>Whether the new value is different to the current value or whether there was a failure.</returns>
        public static bool TryEditValue(this Field field, object value)
        {
            return field.Item.TryEditValue(field.Name, value);
        }

        /// <summary>
        /// Try to edit the fields value
        /// </summary>
        /// <returns>Whether the new value is different to the current value or whether there was a failure.</returns>
        public static bool TryEditValue(this Item item, string index, object value)
        {
            try
            {
                item.Editing.BeginEdit();
                item.Fields[index].Value = value.ToString();
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, typeof(ItemExtensions));
            }
            return item.Editing.EndEdit();
        }
    }
}