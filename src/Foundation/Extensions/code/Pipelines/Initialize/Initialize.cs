using Sitecore.Pipelines;

namespace Lotus.Foundation.Extensions.Pipelines.Initialize
{
    public class Initialize
    {
        public virtual void Process(PipelineArgs args)
        {
            Global.Initialize();
        }
    }
}