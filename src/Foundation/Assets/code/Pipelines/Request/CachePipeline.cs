using System;
using System.Web;

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
                
                context.Response.Cache.SetExpires(DateTime.Now.AddHours(path.GetCacheExpiryHours()));
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error processing cache pipeline", exception);
            }
        }
    }
}