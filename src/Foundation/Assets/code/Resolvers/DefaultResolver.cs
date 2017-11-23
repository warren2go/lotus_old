using System.Web;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;
using Sitecore.Events;

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
                var assetRequest = AssetsRequestHelper.CreateAssetRequest(context, path, relativePath, extension, timestamp);
                
                Event.RaiseEvent("assets:request", new object[1]
                {
                    (object) assetRequest
                });
                
                path.ProcessRequest(assetRequest);
                
                Event.RaiseEvent("assets:request:end", new object[1]
                {
                    (object) assetRequest
                });
                
                context.End();
            }
            else
            {
                context.RedirectIgnored("~/" + relativePath);
            }
        }
    }
}