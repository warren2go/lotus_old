using System.Web;
using Lotus.Foundation.Assets.Paths;

namespace Lotus.Foundation.Assets.Pipelines
{
    public class AssetPipelineArgs
    {
        public HttpContextBase Context { get; set; }
        public IAssetPath Path { get; set; }
        public string RelativePath { get; set; }
        public string Extension { get; set; }
        public int Timestamp { get; set; }
    }
}