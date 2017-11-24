using System.Web;
using Lotus.Foundation.Assets.Paths;

namespace Lotus.Foundation.Assets.Resolvers
{
    public interface IAssetResolver
    {
        IAssetPath GetAssetPath(string relativePath);
        AssetRequest GetAssetRequest(HttpContextBase context, IAssetPath path, string relativePath, string extension, int timestamp);
        void ResolveAsset(HttpContextBase context, string relativePath, string extension);
    }
}