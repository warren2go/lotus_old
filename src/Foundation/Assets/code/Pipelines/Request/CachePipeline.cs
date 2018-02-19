using System;
using System.Web;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.Crypto;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class CachePipeline : IAssetPipeline
    {
        public virtual void Process(AssetPipelineArgs args)
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
                context.Response.Cache.SetValidUntilExpires(true);
                context.Response.Headers.SetHeader("ETag", "{0}:{1}".FormatWith(args.Context.Request.RawUrl.ToMD5(), args.Timestamp.ToHex(true).ToLower()));
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error processing cache pipeline", exception);
            }
        }
    }
}