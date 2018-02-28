using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.SitecoreExtensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using ItemUtil = Lotus.Foundation.Kernel.Utils.SitecoreUtils.ItemUtil;

namespace Lotus.Feature.TwitterCards.Utils
{
    /// <summary>
    /// Refer to http://ogp.me/ when extending
    /// </summary>
    public static class TwitterCardsUtil
    {
        public const string DefaultLocale = "en_US";
        
        public static string GetPageLocale(Item item, string fieldName = "ogLocale", bool useItemLanguage = false)
        {
            if (item == null)
                return string.Empty;
            var locale = item.GetValueFromLookup<string>(fieldName);
            if (useItemLanguage)
                locale = item.Language.CultureInfo.TwoLetterISOLanguageName;
            if (string.IsNullOrEmpty(locale))
                return DefaultLocale;
            return locale.Contains("_") ? locale : "en_{0}".FormatWith(locale);
        }
        
        public static string GetPageTitle(Item item, string fieldName = "ogTitle")
        {
            if (item == null)
                return string.Empty;
            return ItemUtil.GetValueFromLookup<string>(item, fieldName);
        }

        public static string GetPageDescription(Item item, string fieldName = "ogDescription")
        {
            if (item == null)
                return string.Empty;
            return ItemUtil.GetValueFromLookup<string>(item, fieldName);
        }

        public static string GetPageUrl()
        {
            return Sitecore.Web.WebUtil.GetRequestUri().AbsoluteUri;
        }

        public static string GetPageType(Item item, string fieldName = "ogType", string @default = "website")
        {
            if (item == null)
                return @default;
            return ItemUtil.GetValueFromLookup(item, fieldName, @default);
        }

        public static NameValueCollection GetPageTypeExtended(Item item, string fieldName = "ogTypeExtended", bool allowNull = true)
        {
            if (item == null)
                return allowNull ? null : new NameValueCollection();
            return item.GetNameValueListCollection(fieldName);
        }

        public static string GetSiteName(Item item, string fieldName = "ogSiteName")
        {
            if (item == null)
                return string.Empty;
            return ItemUtil.GetValueFromLookup<string>(item, fieldName);
        }

        public static ImageField GetImage(Item item, string fieldName = "ogImage")
        {
            return item.Fields[fieldName];
        }

        public static string GetImageUrl(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            return item.GetImageFieldUrl(fieldName);
        }

        public static string GetImageUrl(ImageField field)
        {
            if (field == null)
                return string.Empty;
            var url = field.GetImageFieldUrl();
            if (url.StartsWith("//"))
            {
                url = "http:{0}".FormatWith(url);
            }
            if (url.StartsWith("https://"))
            {
                url = url.Replace("https://", "http://");
            }
            if (!url.StartsWith("//") && url.StartsWith("/"))
            {
                if (HttpContext.Current != null && Sitecore.Context.Site != null)
                {
                    if (HttpContext.Current.Request.IsSecureConnection)
                    {
                        url = "http://{0}{1}".FormatWith(Sitecore.Context.Site.TargetHostName, url);
                    }
                }
            }
            return url;
        }

        public static string GetImageSecureUrl(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            return GetImageSecureUrl(item.Fields[fieldName]);
        }

        public static string GetImageSecureUrl(ImageField field)
        {
            if (field == null)
                return string.Empty;
            var url = field.GetImageFieldUrl();
            if (url.StartsWith("//"))
            {
                url = "https:{0}".FormatWith(url);
            }
            if (url.StartsWith("http://"))
            {
                url = url.Replace("http://", "https://");
            }
            if (!url.StartsWith("//") && url.StartsWith("/"))
            {
                if (HttpContext.Current != null && Sitecore.Context.Site != null)
                {
                    if (!HttpContext.Current.Request.IsSecureConnection)
                    {
                        url = "https://{0}{1}".FormatWith(Sitecore.Context.Site.TargetHostName, url);
                    }
                }
            }
            return url;
        }

        public static string GetImageWidth(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            return GetImageWidth(item.Fields[fieldName]);
        }

        public static string GetImageWidth(ImageField field)
        {
            if (field == null)
                return string.Empty;
            return field.Width;
        }

        public static string GetImageHeight(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            return item.GetImageField(fieldName).Height;
        }

        public static string GetImageHeight(ImageField field)
        {
            if (field == null)
                return string.Empty;
            return field.Height;
        }

        public static string GetImageMimeType(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            var mediaItem = item.GetMediaItem(fieldName);
            return mediaItem == null ? string.Empty : mediaItem.MimeType;
        }

        public static string GetImageMimeType(ImageField field)
        {
            if (field == null)
                return string.Empty;
            var mediaItem = (MediaItem)field.MediaItem;
            return mediaItem == null ? string.Empty : mediaItem.MimeType;
        }

        public static string GetImageAlt(Item item, string fieldName = "ogImage")
        {
            if (item == null)
                return string.Empty;
            var mediaItem = item.GetMediaItem(fieldName);
            return mediaItem == null ? string.Empty : mediaItem.Alt;
        }

        public static string GetImageAlt(ImageField field)
        {
            if (field == null)
                return string.Empty;
            var mediaItem = (MediaItem)field.MediaItem;
            return mediaItem == null ? string.Empty : mediaItem.Alt;
        }

        public static HtmlString RenderOpenGraph(Item item)
        {
            if (HttpContext.Current == null) return new HtmlString(string.Empty);
            //todo: foreach element on item, combine into a HtmlString object to be written to stream
            return new HtmlString(Environment.NewLine + "<meta property=\"{0}\" content=\"{1}\" />" + Environment.NewLine);
        }
    }
}