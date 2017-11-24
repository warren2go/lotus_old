using System.IO;
using System.Web;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Assets.Pipelines;
using Sitecore.IO;

namespace Lotus.Foundation.Assets
{
    public class AssetRequest
    {
        public HttpContextBase Context { get; set; }
        public IAssetPath Path { get; set; }
        public string RelativePath { get; set; }
        public string Extension { get; set; }
        public int Timestamp { get; set; }
        public FileInfo FileInfo { get; private set; }

        public static implicit operator AssetPipelineArgs(AssetRequest request)
        {
            return new AssetPipelineArgs()
            {
                Context = request.Context,
                Path = request.Path,
                RelativePath = request.RelativePath,
                Extension =  request.Extension,
                Timestamp = request.Timestamp
            };
        }

        public void SetFileInfo()
        {
            var path = FileUtil.MapPath(Context, RelativePath);
            if (!string.IsNullOrEmpty(path))
            {
                FileInfo = new FileInfo(path);
            }
        }
    }
}