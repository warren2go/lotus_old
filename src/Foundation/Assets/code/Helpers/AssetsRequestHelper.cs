using System.IO;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Date;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Foundation.Assets.Helpers
{
    public static class AssetsRequestHelper
    {
        public static string MimeMapper(string extension, bool allowNull = true)
        {
            var customMime = Global.Repository.MimeMapping.TryGetValueOrDefault(extension);
            return allowNull ? customMime : (customMime ?? MimeMapping.GetMimeMapping(extension));
        }
        
        public static bool FileExists(HttpContextBase context, string path, bool relative = true)
        {
            var resolvedPath = relative ? context.Server.MapPath("~") + path : path;
            return File.Exists(resolvedPath);
        }
        
        public static int ExtractTimeoutFromQuerystring(HttpContextBase context)
        {
            var querystring = context.Request.Url.Query;
            return querystring.ExtractPattern<int>(@"\btimeout=(\d+)");
        }
        
        public static int ExtractTimestampFromFile(HttpContextBase context, string path, bool relative = true)
        {
            var resolvedPath = relative ? context.Server.MapPath("~") + path : path;
            return File.Exists(resolvedPath) ? new FileInfo(resolvedPath).LastAccessTimeUtc.ToUnixTimestamp() : 0;
        }
        
        public static int ExtractTimestampFromRelativePath(HttpContextBase context, string relativePath, string extension)
        {
            return relativePath.ExtractPattern<int>(AssetsSettings.Regex.Timestamp.Replace("$extension", extension.Escape()));
        }
        
        public static IAssetPath CreatePathWithRelativePath(string relativePath)
        {
            return Global.Repository.GetFolderPathByRelativePath(relativePath) ?? Global.Repository.GetExtensionPathByExtension(Path.GetExtension(relativePath));
        }

        public static AssetRequest CreateAssetRequest(HttpContextBase context, IAssetPath path, string relativePath, string extension, int timestamp)
        {
            return new AssetRequest()
            {
                Context = context,
                Path = path,
                RelativePath = relativePath,
                Extension =  extension,
                Timestamp = timestamp
            };
        }
    }
}