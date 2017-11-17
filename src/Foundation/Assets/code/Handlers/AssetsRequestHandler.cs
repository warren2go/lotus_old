using System;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.String;
using Lotus.Foundation.Extensions.Web;

namespace Lotus.Foundation.Assets.Handlers
{
    public class AssetsRequestHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var extension = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.Extension);
                var relativePath = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.RelativePath.Replace("$extension", extension.Escape()));
                
                if (!Global.Initialized)
                {
                    #if DEBUG
                    AssetsLogger.Error("Error processing asset [{0}] - handler not initialized".FormatWith(context.Request.Url.AbsolutePath));
                    #endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                if (!AssetsSettings.Enabled)
                {
                    #if DEBUG
                    AssetsLogger.Error("Error processing asset [{0}] - handler disabled".FormatWith(context.Request.Url.AbsolutePath));
                    #endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                Global.Resolver.ResolveAsset(context, relativePath, extension);
            }
            catch (Exception exception)
            {
                AssetsLogger.Error("Error processing asset", exception);
            }
            context.NotFound();
        }
    }
}
