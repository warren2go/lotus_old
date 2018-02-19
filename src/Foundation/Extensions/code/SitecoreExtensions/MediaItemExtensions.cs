using System;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.StringExtensions;
using Sitecore.Web;

namespace Lotus.Foundation.Extensions.SitecoreExtensions
{
    public static class MediaItemExtensions
    {
        public static MediaItem GetMediaItem(this Item item, string mediaFieldName)
        {
            var field = item.Fields[mediaFieldName];
            if (field == null)
            {
                Log.Error("Error getting media item from field - field is null {0}[{1}]".FormatWith(item.Paths.FullPath, mediaFieldName), typeof(MediaItemExtensions));
                return null;
            }
            return GetMediaItem(field);
        }

        public static MediaItem GetMediaItem(this Field field)
        {
            if (field.Type.ToLower().Contains("image"))
            {
                return ((ImageField)field).MediaItem;
            }
            else
            {
                return field.GetItemFromLookup();
            }
        }
        
        public static string GetSafeMediaUrl(this Item item, string mediaFieldName, MediaUrlOptions mediaUrlOptions = null)
        {
            return WebUtil.SafeEncode(item.GetMediaUrl(mediaFieldName, mediaUrlOptions).Replace(" ", "-"));
        }
        
        public static string GetMediaUrl(this Item item, string mediaFieldName, MediaUrlOptions mediaUrlOptions = null)
        {
            if (item.Fields[mediaFieldName] == null)
            {
                Log.Error("Error getting media url from field - item is null {0}[{1}]".FormatWith(item.Paths.FullPath, mediaFieldName), typeof(MediaItemExtensions));
                return string.Empty;
            }
            return item.Fields[mediaFieldName].GetMediaUrl(mediaUrlOptions);
        }
        
        public static string GetSafeMediaUrl(this Field field, MediaUrlOptions mediaUrlOptions = null)
        {
            return HttpUtility.UrlEncode(field.GetMediaUrl(mediaUrlOptions).Replace(" ", "-"));
        }

        public static string GetMediaUrl(this Field field, MediaUrlOptions mediaUrlOptions = null)
        {
            var mediaItem = GetMediaItem(field);
            if (mediaItem == null)
            {
                Log.Error("Error getting media from field - field has no media item attached [{0}]".FormatWith(field.Name), typeof(MediaItemExtensions));
                return string.Empty;
            }
            return GetMediaUrl(mediaItem, mediaUrlOptions);
        }
        
        public static string GetSafeMediaUrl(this MediaItem mediaItem, MediaUrlOptions mediaUrlOptions = null, Func<string, string> customReplacer = null)
        {
            if (customReplacer != null)
            {
                return customReplacer.Invoke(mediaItem.GetMediaUrl(mediaUrlOptions));
            }
            return HttpUtility.UrlEncode(mediaItem.GetMediaUrl(mediaUrlOptions).Replace(" ", "-"));
        }
        
        public static string GetMediaUrl(this MediaItem mediaItem, MediaUrlOptions mediaUrlOptions = null)
        {
            try
            {
                return mediaUrlOptions == null ? MediaManager.GetMediaUrl(mediaItem) : MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(MediaItemExtensions));
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the URL for a MediaItem allowing for prefixing of a protocol and/or usage of SSL.
        /// </summary>
        public static string GetMediaUrl(this Item item, string mediaFieldName, bool prefixProtocol, bool useSSL)
        {
            if (item.Fields[mediaFieldName] == null)
            {
                Log.Error("Error getting media url from field - item is null {0}[{1}]".FormatWith(item.Paths.FullPath, mediaFieldName), typeof(MediaItemExtensions));
                return string.Empty;
            }

            return item.Fields[mediaFieldName].GetMediaUrl(prefixProtocol, useSSL);
        }
        
        /// <summary>
        /// Get the URL for an ImageField allowing for prefixing of a protocol and/or usage of SSL.
        /// </summary>
        public static string GetMediaUrl(this Field field, bool prefixProtocol, bool useSSL)
        {
            MediaItem mediaItem = field.GetItemFromLookup();

            if (mediaItem == null)
            {
                Log.Error("Error getting media from field - field has no media item attached [{0}]".FormatWith(field.Name), typeof(MediaItemExtensions));
                return string.Empty;
            }

            return mediaItem.GetMediaUrl(prefixProtocol, useSSL);
        }

        public static string GetMediaUrl(this MediaItem mediaItem, bool prefixProtocol, bool useSSL)
        {
            try
            {
                MediaUrlOptions muo = new MediaUrlOptions();
                muo.AlwaysIncludeServerUrl = false;

                var mediaUrl = MediaManager.GetMediaUrl(mediaItem, muo);

                if (prefixProtocol)
                {
                    if (!mediaUrl.StartsWith("http") && !mediaUrl.StartsWith(@"//"))
                    {
                        mediaUrl = @"http://" + mediaUrl;
                    }

                    if (mediaUrl.StartsWith(@"//"))
                    {
                        mediaUrl = @"http:" + mediaUrl;
                    }
                }

                return useSSL ? mediaUrl.Replace(@"http://", @"https://") : mediaUrl;
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(MediaItemExtensions));
            }
            return string.Empty;
        }
    }
}