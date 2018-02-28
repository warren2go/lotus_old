using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.SitecoreExtensions
{
    public static class ImageFieldExtensions
    {
        public static ImageField GetImageField(this Item item, string imageFieldName)
        {
            if (item.Fields[imageFieldName] == null)
                return default(ImageField);
            return item.Fields[imageFieldName];
        }

        public static string GetImageFieldUrl(this Item item, string imageFieldName)
        {
            if (item.Fields[imageFieldName] == null)
                return string.Empty;
            return item.Fields[imageFieldName].GetImageFieldUrl();
        }

        public static string GetImageFieldUrl(this Field field)
        {
            var imageField = (ImageField)field;
            return imageField.GetImageFieldUrl();
        }

        public static string GetImageFieldUrl(this ImageField imageField)
        {
            if (imageField.MediaItem == null)
                return string.Empty;
            return MediaManager.GetMediaUrl(imageField.MediaItem);
        }
        
        public static string GetImageFieldUrl(this ImageField imageField, [CanBeNull] MediaUrlOptions mediaUrlOptions)
        {
            if (imageField.MediaItem == null)
                return string.Empty;
            return MediaManager.GetMediaUrl(imageField.MediaItem, mediaUrlOptions ?? MediaUrlOptions.Empty);
        }
    }
}