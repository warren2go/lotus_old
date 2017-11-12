using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Pipelines;

namespace Lotus.Foundation.Assets.Pipelines.Initialize
{
    public class AssetHandler
    {
        public virtual void Process(PipelineArgs args)
        {
            Global.Initialize();
        }
    }
}
