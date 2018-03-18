using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Lotus.Foundation.Kernel.Extensions.Casting;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Utils.SitecoreUtils;
using Sitecore;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Web;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
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
        public static bool HasTemplate(this Item item, params string[] templateIdStrings)
        {
            return templateIdStrings.Any(x => x.Equals(item.TemplateID.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
        
        /// <summary>
        /// Get the item directly from the field via a lookup
        /// </summary>
        [CanBeNull]
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
        [CanBeNull]
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
        [CanBeNull]
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
        [CanBeNull]
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

        [NotNull]
        public static IEnumerable<Item> GetDescendentsWithComparer(this Item item, Func<Item, bool> comparer)
        {
            return item.GetDescendents().Where(comparer);
        }

        [NotNull]
        public static IEnumerable<Item> GetDescendentsWithTemplate(this Item item, string templateid)
        {
            var decendents = new List<Item>(item.GetChildren(ChildListOptions.SkipSorting).Where(x => x.HasTemplate(templateid ?? string.Empty)));
            foreach (Item child in item.Children)
            {
                decendents.AddRange(child.GetDescendentsWithTemplate(templateid));
            }
            return decendents;
        }

        [NotNull]
        public static IEnumerable<Item> GetDescendents(this Item item, bool recursive = true)
        {
            if (recursive)
            {
                var decendents = new List<Item>(item.GetChildren(ChildListOptions.SkipSorting));
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

        [CanBeNull]
        public static IEnumerable<Item> GetDescendentsFromIndex(this Item item)
        {
            return ContentSearchUtil.SearchIndex<SearchResultItem, IEnumerable<Item>>(item.AsIndexable(), items =>
            {
                return items.Where(x => x.Paths.Contains(item.ID)).Select(x => x.GetItem()).ToList();
            });
        }
        
        [CanBeNull]
        public static IEnumerable<Item> GetDescendentsFromIndex(this Item item, string indexName)
        {
            var index = ContentSearchManager.GetIndex(indexName);
            if (index == null) return new Item[0];
            return item.GetDescendentsFromIndex(index);
        }

        [CanBeNull]
        public static IEnumerable<Item> GetDescendentsFromIndex(this Item item, [NotNull] ISearchIndex index)
        {
            return ContentSearchUtil.SearchIndex<SearchResultItem, IEnumerable<Item>>(index, items =>
            {
                return items.Where(x => x.Paths.Contains(item.ID)).Select(x => x.GetItem()).ToList();
            });
        }

        [NotNull]
        public static string GetSafeItemUrl(this Item item, [CanBeNull] UrlOptions urlOptions = null, [CanBeNull] Func<string, string> customReplacer = null)
        {
            if (customReplacer != null)
            {
                return customReplacer.Invoke(item.GetItemUrl(urlOptions));
            }
            return WebUtil.SafeEncode(item.GetItemUrl(urlOptions).Replace(" ", "-"));
        }
        
        [NotNull]
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

        [NotNull]
        public static string GetItemUrlPrefixedSecure(this Item item, [CanBeNull] UrlOptions urlOptions, bool prefixProtocol, bool secure)
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
        [CanBeNull]
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
        [CanBeNull]
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

        [NotNull]
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
                item.Fields[index].Value = (value ?? string.Empty).ToString();
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(ItemExtensions));
            }
            return item.Editing.EndEdit();
        }

        /// <summary>
        /// Edit an item with a delegate void
        /// </summary>
        public static void Edit(this Item item, [NotNull] Action<Item> action)
        {
            Assert.ArgumentNotNull(action, nameof(action));
            item.Editing.BeginEdit();
            action.Invoke(item);
            item.Editing.EndEdit();
        }

        /// <summary>
        /// Try editing an item with a delegate void
        /// </summary>
        /// <returns>Whether Sitecore accepted the edit</returns>
        public static bool TryEdit(this Item item, Action<Item> action)
        {
            try
            {
                if (action == null) return false;
                item.Editing.BeginEdit();
                action.Invoke(item);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(ItemExtensions));
            }
            return item.Editing.EndEdit();
        }
    }
}