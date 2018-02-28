﻿using System;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Extensions.Web;
using Lotus.Foundation.Logging;

namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class RedirectPipeline : IAssetPipeline
    {
        public virtual void Process(AssetPipelineArgs args)
        {
            ProcessRedirect(args);
        }
        
        private void ProcessRedirect(AssetPipelineArgs args)
        {
            var redirect = Settings.CDN.Redirect;
            var context = args.Context;
            try
            {
                if (!string.IsNullOrEmpty(redirect))
                {
                    redirect = redirect.ReplacePattern("$(scheme)", context.Request.IsSecureConnection ? "https" : "http");

                    var domain = context.Request.Url.Host.ExtractPattern(@"^(?:\w\.+)?((?:?<=\.)\w\.com.*)$");
                    if (!string.IsNullOrEmpty(domain))
                    {
                        redirect.ReplacePattern("$(domain)", domain);
                    }
                }
            }
            catch (Exception exception)
            {
                LLog.Error("Error handling assets path request", exception);
            }
            if (!string.IsNullOrEmpty(redirect))
            {
                context.Redirect(redirect);
            }
        }
    }
}