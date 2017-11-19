using System.IO.Compression;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;

namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class CompressPipeline : IAssetPipeline
    {
        public void Process(AssetPipelineArgs args)
        {
            ProcessCompress(args);
        }

        private void ProcessCompress(AssetPipelineArgs args)
        {
            var context = args.Context;
            var acceptEncoding = context.Request.Headers.Get("Accept-Encoding");
            if (string.IsNullOrEmpty(acceptEncoding))
                return;
            
            var supported = AssetsSettings.Compression.Supported.ToLower();
            foreach (var compression in supported.Split('|'))
            {
                if (acceptEncoding.IsMatch(compression))
                {
                    SetCompressionFilter(context, compression);
                    return;
                }
            }
        }

        private void SetCompressionFilter(HttpContext context, string compression)
        {
            context.Response.Headers["Vary"] = "Accept-Encoding";
            
            switch (compression)
            {
                case "gzip":
                    context.Response.Headers["Content-Encoding"] = compression;
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress, true);
                    break;
                    
                case "deflate":
                    context.Response.Headers["Content-Encoding"] = compression;
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress, true);
                    break;
            }
        }
    }
}