using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Lotus.Foundation.Extensions.Configuration;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Foundation.Extensions.Web
{
    public static class WebExtensions
    {
      public static bool WriteFile(this HttpContext context, string relativePath, string contentType = null)
        {
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
            
            if (File.Exists(webRoot + relativePath))
            {
                using (var fileStream = File.Open(webRoot + relativePath, FileMode.Open))
                {
                    fileStream.CopyTo(context.Response.OutputStream);
                }

                if (!string.IsNullOrEmpty(relativePath))
                {
                    context.Response.ContentType = contentType ?? MimeMapping.GetMimeMapping(relativePath);   
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static void End(this HttpContext context, bool abortThread = true)
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
        
        public static void NotFound(this HttpContext context, string status = "404 Not Found", bool endResponse = true)
        {
            context.Response.StatusCode = 404;
            context.Response.Status = status;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        public static void InternalServerError(this HttpContext context, string status = "500 Internal Server Error", bool endResponse = true)
        {
            context.Response.StatusCode = 500;
            context.Response.Status = status;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        public static void Redirect(this HttpContext context, string url)
        {
            context.Response.Redirect(url);
        }

        public static void RedirectPermanent(this HttpContext context, string url)
        {
            context.Response.RedirectPermanent(url);
        }

        public static void SetHeader(this NameValueCollection collection, string header, object value = null)
        {
            collection[header] = (value ?? string.Empty).ToString();
        }
    }
}