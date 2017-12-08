using Sitecore.Pipelines;

namespace Lotus.Feature.MailChimp.Pipelines.Initialize
{
    public class Initialize
    {
        public virtual void Process(PipelineArgs args)
        {
            Global.Initialize();
        }
    }
}
