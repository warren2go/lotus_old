using System.Web;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;
using Sitecore.Events;

namespace Lotus.Foundation.Assets.Resolvers
{
    public class DefaultResolver : IAssetResolver
    {
        public IAssetPath GetAssetPath(string relativePath)
        {
            return AssetsRequestHelper.CreatePathWithRelativePath(relativePath);
        }
        
        public AssetRequest GetAssetRequest(HttpContextBase context, IAssetPath path, string relativePath, string extension, int timestamp)
        {
            return AssetsRequestHelper.CreateAssetRequest(context, path, relativePath, extension, timestamp);
        }
        
        public void ResolveAsset(HttpContextBase context, string relativePath, string extension)
        {
            var path = GetAssetPath(relativePath);
            
            var timestamp = AssetsRequestHelper.ExtractTimestampFromRelativePath(context, relativePath, extension);
                
            relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));

            if (path != null)
            {
                var assetRequest = GetAssetRequest(context, path, relativePath, extension, timestamp);
                
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