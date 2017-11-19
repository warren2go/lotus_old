using System.IO;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Date;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.String;

namespace Lotus.Foundation.Assets.Helpers
{
    public static class AssetsRequestHelper
    {
        public static int ExtractTimeoutFromQuerystring(HttpContext context)
        {
            var querystring = context.Request.Url.Query;
            return querystring.ExtractPattern<int>(@"\btimeout=(\d+)");
        }
        
        public static int ExtractTimestampFromFile(HttpContext context, string path, bool relative = true)
        {
            var resolvedPath = relative ? context.Server.MapPath("~") + path : path;
            return File.Exists(resolvedPath) ? new FileInfo(resolvedPath).LastAccessTimeUtc.ToUnixTimestamp() : 0;
        }
        
        public static int ExtractTimestampFromRelativePath(HttpContext context, string relativePath, string extension)
        {
            return relativePath.ExtractPattern<int>(AssetsSettings.Regex.Timestamp.Replace("$extension", extension.Escape()));
        }
        
        public static IAssetPath CreatePathWithRelativePath(string relativePath)
        {
            return Global.Repository.GetFolderPathByRelativePath(relativePath) ?? Global.Repository.GetExtensionPathByExtension(Path.GetExtension(relativePath));
        }
    }
}