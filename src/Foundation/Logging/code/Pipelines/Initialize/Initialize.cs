using Sitecore.Pipelines;

namespace Lotus.Foundation.Logging.Pipelines.Initialize
{
    public class Initialize
    {
        public virtual void Process(PipelineArgs args)
        {
            Global.Initialize();
        }
    }
}