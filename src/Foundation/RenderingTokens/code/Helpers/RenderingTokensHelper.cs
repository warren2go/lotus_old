using Lotus.Foundation.Extensions.Primitives;

namespace Lotus.Foundation.RenderingTokens.Helpers
{
    public static class RenderingTokensHelper
    {
        public static string ResolveTokenName(object model)
        {
            return ResolveTokenName(model.GetType().FullName);
        }
        
        public static string ResolveTokenName(string key)
        {
            return Settings.ResolveTokenFormat.FormatWith(key);
        }
    }
}