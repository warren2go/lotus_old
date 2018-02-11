using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Lotus.Foundation.Extensions.Configuration;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Extensions.Web
{
    public static class WebExtensions
    {
        public static bool WriteFile(this HttpContextBase context, string relativePath, string contentType = null)
        {
            Assert.ArgumentNotNull((object) relativePath, nameof (relativePath));
            
            var webRoot = context.Server.MapPath("~");

            if (relativePath.StartsWith("~"))
            {
                Global.Logger.Warn("RelativePath contains begins with webroot - stripping this {0}[{1}]".FormatWith(relativePath, contentType ?? "null"));
                relativePath = relativePath.ReplacePattern(@"\~", "");
            }
            
            if (relativePath.Contains("..") || relativePath.Contains("://") || relativePath.Contains(@":\\"))
            {
                Global.Logger.Warn("RelativePath contains crawling or invalid charcters - stripping these {0}[{1}]".FormatWith(relativePath, contentType ?? "null"));
                relativePath = relativePath.ReplacePattern(@"\.\.", "");
                relativePath = relativePath.ReplacePattern("://", "");
                relativePath = relativePath.ReplacePattern(@":\\\\", "");
            }

            var path = webRoot + relativePath;
            
            if (File.Exists(path))
            {
                context.Response.ContentType = contentType ?? MimeMapping.GetMimeMapping(path);
                context.Response.TransmitFile(path);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static void End(this HttpContextBase context, bool abortThread = true)
        {
            if (abortThread)
            {
                context.Response.End();
            }
            else
            {
                var rawUrl = context.Request.RawUrl;
                try
                {
                    context.Response.End();
                }
                catch (Exception exception)
                {
                    #if DEBUG
                    Global.Logger.Debug("Terminated request - [{0}]".FormatWith(rawUrl), exception);
                    #endif
                }
            }
        }
        
        public static void NotFound(this HttpContextBase context, string statusDescription = "Not Found", bool endResponse = true)
        {
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = statusDescription;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        public static void InternalServerError(this HttpContextBase context, string statusDescription = "Internal Server Error", bool endResponse = true)
        {
            context.Response.StatusCode = 500;
            context.Response.StatusDescription = statusDescription;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        public static void Redirect(this HttpContextBase context, string url)
        {
            context.Response.Redirect(url);
        }

        public static void RedirectPermanent(this HttpContextBase context, string url)
        {
            context.Response.RedirectPermanent(url);
        }

        public static void SetHeader(this NameValueCollection collection, string header, object value = null)
        {
            Assert.ArgumentNotNull((object) header, nameof (header));
            collection[header] = (value ?? string.Empty).ToString();
        }
    }
}