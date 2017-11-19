using System;
using System.Linq;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Extensions.Regex;
using Lotus.Foundation.Extensions.String;
using Lotus.Foundation.Extensions.Web;
using Sitecore.Diagnostics;

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
                    Log.Debug("Error processing asset [{0}] - handler not initialized".FormatWith(context.Request.Url.AbsolutePath));
                    #endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                if (!AssetsSettings.Enabled)
                {
                    #if DEBUG
                    Global.Logger.Debug("Error processing asset [{0}] - handler disabled".FormatWith(context.Request.Url.AbsolutePath));
                    #endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                if (!Global.Repository.Hosts.Any(x => context.Request.Url.Host.IsMatch(x)))
                {
                    #if DEBUG
                    Global.Logger.Debug("Error processing asset [{0}{1}] - no matching host found".FormatWith(context.Request.Url.Host, context.Request.Url.AbsolutePath));
                    #endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                Global.Resolver.ResolveAsset(context, relativePath, extension);
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error processing asset", exception);
            }
            context.NotFound();
        }
    }
}
