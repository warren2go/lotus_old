using System;
using System.Web;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Extensions.Date;
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
                
            relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));

            if (path != null)
            {
                path.ProcessRequest(context, relativePath, extension, timestamp);
//                        if (!context.WriteFile(relativePath))
//                        {
//                            Global.Logger.Error("Error writing file to http context [{0}]".FormatWith(relativePath));
//                        }
                context.End();
            }
            else
            {
                context.RedirectIgnored("~/" + relativePath);
            }
        }
    }
}