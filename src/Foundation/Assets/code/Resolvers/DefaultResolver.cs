using System.IO;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.String;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets.Resolvers
{
    public class DefaultResolver : IAssetResolver
    {
        public void ResolveAsset(HttpContext context, string relativePath, string extension)
        {
            var path = AssetsRequestHelper.CreatePathWithRelativePath(relativePath);

            var timestamp = AssetsRequestHelper.ExtractTimestampFromRelativePath(context, relativePath, extension);
                
            if (timestamp > 0)
            {
                relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
                    
                if (path != null)
                {
                    path.ProcessRequest(context, relativePath, extension, timestamp);
                }
                else
                {
                    if (!context.WriteFile(relativePath))
                    {
                        AssetsLogger.Error("Error writing file to http context [{0}]".FormatWith(relativePath));
                    }
                }
                context.End();
            }
            else
            {
                relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
                    
                if (path != null)
                {
                    path.ProcessRequest(context, relativePath, extension, timestamp);
                }
                else
                {
                    if (!context.WriteFile(relativePath))
                    {
                        AssetsLogger.Error("Error writing file to http context [{0}]".FormatWith(relativePath));
                    }
                }
                context.End();
            }
        }
    }
}