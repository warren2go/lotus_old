using System.Web;

namespace Lotus.Foundation.Assets.Resolvers
{
    public interface IAssetResolver
    {
        void ResolveAsset(HttpContext context, string relativePath, string extension);
    }
}