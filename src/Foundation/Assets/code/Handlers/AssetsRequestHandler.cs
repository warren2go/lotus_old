using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions;

namespace Lotus.Foundation.Assets.Handlers
{
    public class AssetsRequestHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var extension = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.Extension);
                var relativePath = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.RelativePath.Replace("$extension", extension.Escape()));
                
                if (!Global.Initialized || !AssetsSettings.Enabled)
                {
                    RedirectIgnored(context, "~/" + relativePath);
                }

                var path = CreatePathWithRelativePath(relativePath);

                var timestamp = ExtractTimestamp(context, relativePath, extension);
                
                if (timestamp > 0)
                {
                    relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
                    
                    if (path != null)
                    {
                        InjectAssetWithPath(context, path, relativePath);
                    }
                    else
                    {
                        InjectAsset(context, relativePath);
                    }
                    End(context);
                }
                else
                {
                    relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
                    
                    if (path != null)
                    {
                        var fileName = relativePath.ExtractPattern(AssetsSettings.Regex.FileName);

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            if (path.GetFileNames().Any(x => !string.IsNullOrEmpty(x) && (Regex.IsMatch(fileName, x) || x.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))))
                            {
                                HandleAssetWithPath(context, path, relativePath, fileName, extension, timestamp);
                                End(context);
                            }
                        }
                    }
                    else
                    {
                        InjectAsset(context, relativePath);
                        End(context);
                    }
                }
            }
            catch (ThreadAbortException exception)
            {
                AssetsLogger.Warn("Aborted thread while processing asset", exception);
                return;
            }
            catch (Exception exception)
            {
                AssetsLogger.Error("Error processing asset", exception);
            }
            
            NotFound(context);
        }

        private int ExtractTimestamp(HttpContext context, string relativePath, string extension)
        {
            return relativePath.ExtractPattern<int>(AssetsSettings.Regex.Timestamp.Replace("$extension", extension.Escape()));
        }

        private void InjectAssetWithPath(HttpContext context, IAssetPath path, string relativePath)
        {
            InjectCachingIntoResponse(context, path);
            InjectAsset(context, relativePath);
        }
        
        private void InjectAsset(HttpContext context, string relativePath)
        {
            var webRoot = context.Server.MapPath("~");
            
            if (!string.IsNullOrEmpty(webRoot) && File.Exists(webRoot + relativePath))
            {
                using (var fileStream = File.Open(webRoot + relativePath, FileMode.Open))
                {
                    fileStream.CopyTo(context.Response.OutputStream);
                }

                if (!string.IsNullOrEmpty(relativePath))
                {
                    context.Response.ContentType = MimeMapping.GetMimeMapping(relativePath);   
                }
            }
            else
            {
                AssetsLogger.Warn("Asset not found with path = {0} -> {1}".FormatWith(webRoot + relativePath, webRoot));
                NotFound(context);
            }
        }
        
        private void HandleAssetWithPath(HttpContext context, IAssetPath path, string relativePath, string fileName, string extension, int timestamp)
        {   
            var ignore = path.GetIgnore();
            foreach (var ignored in ignore)
            {
                if (!string.IsNullOrEmpty(ignored) && fileName.IsMatch(ignored))
                {
                    RedirectIgnored(context, "~/" + relativePath);
                }
            }

            var modified = ExtractDateModifiedFromFile(context, relativePath);

            if (timestamp != modified)
            {
                RedirectWithUpdate(context, modified, relativePath, extension);
            }
            else
            {
                InjectAssetWithPath(context, path, relativePath);
            }
        }

        private void RedirectWithUpdate(HttpContext context, int timestamp, string relativePath, string extension)
        {
            Redirect(context, "~/-/assets/{0}".FormatWith(relativePath.ReplacePattern(extension.Escape(), "-{0:0000000000}{1}".FormatWith(timestamp, extension))));
        }
        
        private int ExtractDateModifiedFromFile(HttpContext context, string relativePath)
        {
            var webRoot = context.Server.MapPath("~");
            if (File.Exists(webRoot + relativePath))
            {
                return new FileInfo(webRoot + relativePath).LastAccessTimeUtc.ToUnixTimestamp();
            }
            return 0;
        }

        private void InjectCachingIntoResponse(HttpContext context, IAssetPath path)
        {
            try
            {
                var expireHours = AssetsSettings.Caching.DefaultExpireHours;

                if (path != null)
                {
                    expireHours = path.GetExpireCache();
                }
                
                context.Response.Cache.SetExpires(DateTime.Now.AddHours(expireHours));
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
            }
            catch (Exception exception)
            {
                AssetsLogger.Error("Error handling assets caching request", exception);
            }
        }
        
        private string ResolvePrefix(HttpContext context, IAssetPath path)
        {
            var cdnPrefix = AssetsSettings.CDN.Prefix;
            try
            {
                if (!string.IsNullOrEmpty(cdnPrefix))
                {
                    cdnPrefix = Regex.Replace(cdnPrefix, "$scheme", context.Request.IsSecureConnection ? "https" : "http");

                    var host = context.Request.Url.Host;
                    var match = Regex.Match(host, @"^((\w\.)+(\w\.com.*))$");
                    if (match.Success && match.Groups.Count >= 4)
                    {
                        cdnPrefix = Regex.Replace(cdnPrefix, "$subdomain", match.Groups[1].Value);
                    }

                    match = Regex.Match(host, @"^((\w\.)*(\w\.com.*))$");
                    if (match.Success && match.Groups.Count >= 4)
                    {
                        cdnPrefix = Regex.Replace(cdnPrefix, "$domain", match.Groups[3].Value);
                    }
                }
            }
            catch (Exception exception)
            {
                AssetsLogger.Error("Error handling assets path request", exception);
            }
            return cdnPrefix;
        }

        private IAssetPath CreatePathWithRelativePath(string relativePath)
        {
            return Global.Repository.GetFolderPathByUri(relativePath) ?? Global.Repository.GetExtensionPathByExtension(Path.GetExtension(relativePath));
        }

        private void End(HttpContext context)
        {
            try
            {
                context.Response.End();
            }
            catch { }
        }

        private void Redirect(HttpContext context, string url)
        {
            context.Response.Redirect(url);
        }

        private void RedirectIgnored(HttpContext context, string url)
        {
            if (AssetsSettings.IgnoreType.IsMatch("querystring"))
            {
                if (!url.Contains("?"))
                {
                    url += "?ignore=true";
                }
                else
                {
                    url += "&ignore=true";
                }   
            }
            if (AssetsSettings.IgnoreType.IsMatch("timestamp"))
            {
                if (url.IsMatch(AssetsSettings.Regex.Timestamp))
                {
                    url = url.ReplacePattern(AssetsSettings.Regex.Timestamp, "0000000000");
                }
                else
                {
                    var extension = url.ExtractPattern(AssetsSettings.Regex.Extension);
                    url = url.ReplacePattern(extension.Escape(), "-{0}{1}".FormatWith("0000000000", extension));
                }
            }
            context.Response.Redirect(url);
        }

        private void NotFound(HttpContext context, string status = "404 Not Found")
        {
            context.Response.StatusCode = 404;
            context.Response.Status = status;
        }

        private void InternalServerError(HttpContext context, string status = "500 Internal Server Error")
        {
            context.Response.StatusCode = 500;
            context.Response.Status = status;           
        }
    }
}
