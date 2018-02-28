using System.Linq;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;

namespace Lotus.Foundation.Assets.Paths.File
{
    public class FilePath : BasePath
    {
        public string ParentPath { get; set; }

        public override void ProcessRequest(AssetRequest request)
        {
            var fileName = request.RelativePath.ExtractPattern(Settings.Regex.FileName);
            if (!GetTargets().Any(x => CheckTarget(fileName, x)))
            {
                request.Context.RedirectBad("~/" + request.RelativePath);
            }
            base.ProcessRequest(request);
        }
    }
}