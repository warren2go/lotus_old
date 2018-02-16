using System.IO.Compression;
using System.Linq;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;

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

            var mimeType = AssetsRequestHelper.MimeMapper(args.Extension, false);

            if (Settings.Compression.MimeTypes.ToLower().Split('|').Any(x => x.IsMatch(mimeType.Escape())))
            {
                var compression = Settings.Compression.AcceptEncoding.ToLower().Split('|')
                    .FirstOrDefault(x => acceptEncoding.IsMatch(x));
                
                if (!string.IsNullOrEmpty(compression))
                {
                    SetCompressionFilter(context, compression);
                }
            }
        }

        private void SetCompressionFilter(HttpContextBase context, string compression)
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