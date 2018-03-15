using System;
using System.Linq;
using Lotus.Foundation.Kernel.Extensions.SitecoreExtensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Sitecore;
using Sitecore.Data;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class ItemUtil
    {
        /// <summary>
        /// Whether a Sitecore Item isnt null and has the specified Field.
        /// </summary>
        public static bool HasField(Item item, string fieldName)
        {
            return item != null && item.Fields[fieldName] != null;
        }

        public static bool HasTemplate(Item item, params ID[] templateIds)
        {
            return item != null && item.HasTemplate(templateIds);
        }
        
        public static bool HasTemplate(Item item, params string[] templateIdStrings)
        {
            return item != null && item.HasTemplate(templateIdStrings);
        }
        
        /// <summary>
        /// Compare <see cref="Item"/>s using their instances
        /// </summary>
        public static bool CompareItems(params Item[] items)
        {
            return CompareItems(delegate (Item[] compare)
            {
                var first = compare.FirstOrDefault();
                return compare.All(item => first != null && item != null && first.ID == item.ID);
            }, items);
        }

        /// <summary>
        /// Compare <see cref="Item"/>s using their instances and a predicate
        /// </summary>
        public static bool CompareItems(Predicate<Item[]> predicate, params Item[] items)
        {
            return predicate.Invoke(items);
        }

        /// <summary>
        /// Get the item directly from the field via a lookup. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The item after performing the lookup, or null if failing.</returns>
        [CanBeNull]
        public static Item GetItemFromLookup(Item item, string fieldName)
        {
            if (!HasField(item, fieldName))
            {
                return null;
            }
            return item.GetItemFromLookup(fieldName);
        }

        /// <summary>
        /// Get the item directly from the field via a lookup. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The item after performing the lookup, or null if failing.</returns>
        [CanBeNull]
        public static Item GetItemFromLookup(Field field)
        {
            if (field == null)
            {
                return null;
            }
            return field.GetItemFromLookup();
        }

        /// <summary>
        /// Get the items directly from the field via a multilist field. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The items after casting to a multilist field. Will return null if <see cref="allowNull"/> is true or an empty collection if false.</returns>
        [CanBeNull]
        public static Item[] GetItemsFromMultilist(Item item, string fieldName, bool allowNull = true)
        {
            if (!HasField(item, fieldName))
            {
                return allowNull ? null : new Item[0];
            }
            return item.GetItemsFromMultilist(fieldName, allowNull);
        }

        /// <summary>
        /// Get the items directly from the field via a multilist field. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The items after casting to a multilist field. Will return null if <see cref="allowNull"/> is true or an empty collection if false.</returns>
        [CanBeNull]
        public static Item[] GetItemsFromMultilist(Field field, bool allowNull = true)
        {
            if (field == null)
            {
                return allowNull ? null : new Item[0];
            }
            return field.GetItemsFromMultilist(allowNull);
        }

        /// <summary>
        /// Get the value from a Sitecore Item via the specified Field. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The value stored in the Field or the @default value if this process fails.</returns>
        [CanBeNull]
        public static T GetValueFromLookup<T>(Item item, string fieldName, T @default = default(T))
        {
            if (!HasField(item, fieldName))
            {
                return @default;
            }
            return item.GetValueFromLookup(fieldName, @default);
        }

        /// <summary>
        /// Get the value from a Sitecore Field. See <seealso cref="Extensions"></seealso> for alternate usage.
        /// </summary>
        /// <returns>The value stored in the Field or the @default value if this process fails.</returns>
        [CanBeNull]
        public static T GetValueFromLookup<T>(Field field, T @default = default(T))
        {
            if (field == null)
            {
                return @default;
            }
            return field.GetValueFromLookup(@default);
        }

        /// <summary>
        /// Resolve the URL associated with an item
        /// </summary>
        [NotNull]
        public static string GetItemUrl(Item item, UrlOptions urlOptions = null, string @default = "")
        {
            if (item == null)
            {
                return @default;
            }
            return item.GetItemUrl(urlOptions);
        }

        /// <summary>
        /// Resole the URL associated with an image field (using the media item)
        /// </summary>
        [NotNull]
        public static string GetImageFieldUrl(ImageField imageField, MediaUrlOptions mediaUrlOptions = null)
        {
            if (imageField == null)
                return string.Empty;
            return imageField.GetImageFieldUrl(mediaUrlOptions);
        }

        [CanBeNull]
        public static Item GetContextItem(RenderingContext renderingContext)
        {
            return renderingContext != null ? renderingContext.ContextItem : null;
        }
    }
}