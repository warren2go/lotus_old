using System.Web;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Extensions.Web;
using Sitecore.Events;

namespace Lotus.Foundation.Assets.Resolvers
{
    public class DefaultResolver : IAssetResolver
    {
        public IAssetPath GetAssetPath(string relativePath)
        {
            return AssetsRequestHelper.CreatePathWithRelativePath(relativePath);
        }
        
        public AssetRequest GenerateAssetRequest(HttpContextBase context, IAssetPath path, string relativePath, string extension, int timestamp)
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
                var assetRequest = GenerateAssetRequest(context, path, relativePath, extension, timestamp);
                
                Event.RaiseEvent("assets:request", assetRequest);
                
                path.ProcessRequest(assetRequest);
                
                Event.RaiseEvent("assets:request:end", assetRequest);
                
                context.End();
            }
            else
            {
                context.RedirectIgnored("~/" + relativePath);
            }
        }
    }
}