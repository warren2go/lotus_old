using Lotus.Foundation.Extensions.Primitives;

namespace Lotus.Foundation.RenderingTokens.Helpers
{
    public static class RenderingTokensHelper
    {
        public static string ResolveTokenName(object model)
        {
            return Settings.ResolveTokenFormat.FormatWith(model.GetType().FullName);
        }
    }
}