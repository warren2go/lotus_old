using System.Linq;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;

namespace Lotus.Foundation.Assets.Paths.File
{
    public class FilePath : BasePath
    {
        public string ParentPath { get; set; }

        public override void ProcessRequest(HttpContext context, string relativePath, string extension, int timestamp)
        {
            var fileName = relativePath.ExtractPattern(AssetsSettings.Regex.FileName);
            if (!GetTargets().Any(x => CheckTarget(fileName, x)))
            {
                context.RedirectBad("~/" + relativePath);
            }
            base.ProcessRequest(context, relativePath, extension, timestamp);
        }
    }
}