using System;
using Lotus.Foundation.Kernel.Extensions.SitecoreExtensions;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel.Utils.SitecoreUtils
{
    public static class ItemEditingUtil
    {
        /// <summary>
        /// Edit the value on an item with a converter
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void EditValue<T>([NotNull] Item item, string fieldName, T value, Func<object, string> converter = null)
        {
            Assert.ArgumentNotNull(item, nameof(item));
            Assert.ArgumentNotNull(fieldName, nameof(fieldName));
            EditValue<T>(item.Fields[fieldName], value, converter);
        }
        
        /// <summary>
        /// Try editing the value on an item with a converter, catching the exception in the process
        /// </summary>
        public static bool TryEditValue<T>([NotNull] Item item, string fieldName, T value, Func<object, string> converter = null)
        {
            try
            {
                EditValue<T>(item, fieldName, value, converter);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Error editing value on item", exception, typeof(ItemEditingUtil));
                return false;
            }
        }
        
        /// <summary>
        /// Edit the value on an field with a converter
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void EditValue<T>([NotNull] Field field, T value, Func<object, string> converter = null)
        {
            Assert.ArgumentNotNull(field, nameof(field));
            var item = field.Item;
            if (item.Editing.IsEditing)
            {
                if (converter == null)
                    converter = @object => (@object ?? string.Empty).ToString();
                field.Value = converter.Invoke(value);
            }
        }

        /// <summary>
        /// Try editing the value on an field with a converter, catching the exception in the process
        /// </summary>
        public static bool TryEditValue<T>([NotNull] Field field, T value, Func<object, string> converter = null)
        {
            try
            {
                EditValue<T>(field, value, converter);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Error editing value on field", exception, typeof(ItemEditingUtil));
                return false;
            }
        }
        
        /// <summary>
        /// Edit an item with a delegate void
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void Edit([NotNull] Item item, [NotNull] Action<Item> action)
        {
            Assert.ArgumentNotNull(item, nameof(item));
            item.Edit(action);
        }

        /// <summary>
        /// Try edit an item with a delegate void
        /// </summary>
        public static bool TryEdit([NotNull] Item item, [NotNull] Action<Item> action)
        {
            if (item == null) return false;
            return item.TryEdit(action);
        }
    }
}