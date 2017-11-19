using System;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class RedirectPipeline : IAssetPipeline
    {
        public void Process(AssetPipelineArgs args)
        {
            ProcessRedirect(args);
        }
        
        private void ProcessRedirect(AssetPipelineArgs args)
        {
            var redirect = AssetsSettings.CDN.Redirect;
            var context = args.Context;
            try
            {
                if (!string.IsNullOrEmpty(redirect))
                {
                    redirect = redirect.ReplacePattern("$scheme", context.Request.IsSecureConnection ? "https" : "http");

                    var domain = context.Request.Url.Host.ExtractPattern(@"^(?:\w\.+)?((?:?<=\.)\w\.com.*)$");
                    if (!string.IsNullOrEmpty(domain))
                    {
                        redirect.ReplacePattern("$domain", domain);
                    }
                }
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error handling assets path request", exception);
            }
            if (!string.IsNullOrEmpty(redirect))
            {
                context.Redirect(redirect);
            }
        }
    }
}