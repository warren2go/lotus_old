using Lotus.Foundation.RenderingTokens.Helpers;
using Sitecore.Pipelines;

namespace Lotus.Foundation.RenderingTokens.Structures
{
    public class TokenRenderingArgs : PipelineArgs
    {
        public void Add(object model)
        {
            CustomData.Add(RenderingTokensHelper.ResolveTokenName(model), model);
        }
    }
}