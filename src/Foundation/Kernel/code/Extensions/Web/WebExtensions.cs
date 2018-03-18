using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Sitecore;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel.Extensions.Web
{
    public static class WebExtensions
    {
        public static bool WriteFile(this HttpContextBase context, [NotNull] string relativePath, [CanBeNull] string contentType = null)
        {
            Assert.ArgumentNotNull((object) relativePath, nameof (relativePath));
            
            var webRoot = context.Server.MapPath("~");

            if (relativePath.StartsWith("~"))
            {
                Log.Warn("RelativePath contains begins with webroot - stripping this {0}[{1}]".FormatWith(relativePath, contentType ?? "null"), typeof(WebExtensions));
                relativePath = relativePath.ReplacePattern(@"\~", "");
            }
            
            if (relativePath.Contains("..") || relativePath.Contains("://") || relativePath.Contains(@":\\"))
            {
                Log.Warn("RelativePath contains crawling or invalid charcters - stripping these {0}[{1}]".FormatWith(relativePath, contentType ?? "null"), typeof(WebExtensions));
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
        
        [TerminatesProgram]
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
                catch
                {
                    #if DEBUG
                    Log.Debug("Terminated request - [{0}]".FormatWith(rawUrl), typeof(WebExtensions));
                    #endif
                }
            }
        }
        
        [TerminatesProgram]
        public static void NotFound(this HttpContextBase context, string statusDescription = "Not Found", bool endResponse = true)
        {
            context.Response.StatusCode = 404;
            context.Response.StatusDescription = statusDescription;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        [TerminatesProgram]
        public static void InternalServerError(this HttpContextBase context, string statusDescription = "Internal Server Error", bool endResponse = true)
        {
            context.Response.StatusCode = 500;
            context.Response.StatusDescription = statusDescription;
            
            if (endResponse)
            {
                context.Response.End();
            }
        }

        [TerminatesProgram]
        public static void Redirect(this HttpContextBase context, [NotNull] string url)
        {
            Assert.ArgumentNotNull(url, nameof(url));
            context.Response.Redirect(url);
        }

        [TerminatesProgram]
        public static void RedirectPermanent(this HttpContextBase context, [NotNull] string url)
        {
            Assert.ArgumentNotNull(url, nameof(url));
            context.Response.RedirectPermanent(url);
        }

        public static void SetHeader(this NameValueCollection collection, string header, [CanBeNull] object value = null)
        {
            Assert.ArgumentNotNull((object) header, nameof (header));
            collection[header] = (value ?? string.Empty).ToString();
        }
    }
}