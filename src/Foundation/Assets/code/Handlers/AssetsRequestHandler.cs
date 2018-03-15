using System;
using System.Linq;
using System.Threading;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Extensions.Web;
using Lotus.Foundation.Logging;
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
            Assert.IsNotNull((object)context, nameof(context));
            ProcessRequest((HttpContextBase) new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            Assert.ArgumentNotNull(context, nameof (context));
            
            try
            {
                var extension = context.Request.Url.AbsolutePath.ExtractPattern(Settings.Regex.Extension);
                var relativePath = context.Request.Url.AbsolutePath.ExtractPattern(Settings.Regex.RelativePath.Replace("$(extension)", extension.Escape()));

                if (!Global.Initialized)
                {
#if DEBUG
                    LLog.Debug("Error processing asset [{0}] - handler not initialized".FormatWith(context.Request.Url.AbsolutePath));
#endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                if (!Settings.Enabled)
                {
#if DEBUG
                    if (context.Request.Url != null) LLog.Debug("Error processing asset [{0}] - handler disabled".FormatWith(context.Request.Url.AbsolutePath));
#endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                if (!Global.Repository.Hosts.Any(x => context.Request.Url.Host.IsMatch(x)))
                {
#if DEBUG
                    LLog.Debug("Error processing asset [{0}{1}] - no matching host found".FormatWith(context.Request.Url.Host, context.Request.Url.AbsolutePath));
#endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                var ignored = context.Request.RawUrl.ExtractPattern(Settings.Regex.IgnoreQuery);

                if (!string.IsNullOrEmpty(ignored))
                {
                    var timestamp = AssetsRequestHelper.ExtractTimestampFromRelativePath(context, relativePath, extension);
                    
                    relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
#if DEBUG
                    LLog.Debug("Redirecting ignored asset {0} -> [{1}]".FormatWith(context.Request.RawUrl, relativePath));
#endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                Global.Resolver.ResolveAsset(context, relativePath, extension);
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(ThreadAbortException))
                {
                    LLog.Error("Error processing asset", exception);
                
                    context.NotFound();   
                }
            }
        }
    }
}
