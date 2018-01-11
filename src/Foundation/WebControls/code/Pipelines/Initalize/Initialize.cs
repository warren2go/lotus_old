using Sitecore.Pipelines;

namespace Lotus.Foundation.WebControls.Pipelines.Initalize
{
    public class Initialize
    {
        public virtual void Process(PipelineArgs args)
        {
            Global.Initialize();
        }
    }
}