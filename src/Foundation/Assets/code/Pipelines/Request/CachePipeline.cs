using System;
using System.Web;
using Lotus.Foundation.Extensions.Crypto;
using Lotus.Foundation.Extensions.Web;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class CachePipeline : IAssetPipeline
    {
        public void Process(AssetPipelineArgs args)
        {
            ProcessCache(args);
        }
        
        private void ProcessCache(AssetPipelineArgs args)
        {
            try
            {
                var context = args.Context;
                var path = args.Path;

                context.Response.Cache.SetMaxAge(new TimeSpan(path.GetCacheExpiryHours(), 0, 0));
                context.Response.Cache.SetExpires(DateTime.Now.AddHours(path.GetCacheExpiryHours()));
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Headers.SetHeader("ETag", "{0}:{1}".FormatWith(args.RelativePath, args.Timestamp).ToMD5());
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error processing cache pipeline", exception);
            }
        }
    }
}