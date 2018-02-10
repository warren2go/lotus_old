using System;
using System.Linq;
using System.Threading;
using System.Web;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Helpers;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Web;
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
            Assert.ArgumentNotNull((object) context, nameof (context));
            
            try
            {
                var extension = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.Extension);
                var relativePath = context.Request.Url.AbsolutePath.ExtractPattern(AssetsSettings.Regex.RelativePath.Replace("$(extension)", extension.Escape()));

                if (!Global.Initialized)
                {
#if DEBUG
                    LLog.Debug("Error processing asset [{0}] - handler not initialized".FormatWith(context.Request.Url
                        .AbsolutePath));
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

                var ignored = context.Request.RawUrl.ExtractPattern(AssetsSettings.Regex.IgnoreQuery);

                if (!string.IsNullOrEmpty(ignored))
                {
                    var timestamp = AssetsRequestHelper.ExtractTimestampFromRelativePath(context, relativePath, extension);
                    relativePath = relativePath.ReplacePattern("-{0:0000000000}".FormatWith(timestamp));
#if DEBUG
                    Global.Logger.Debug("Redirecting ignored asset {0} -> [{1}]".FormatWith(context.Request.RawUrl, relativePath));
#endif
                    context.RedirectIgnored("~/" + relativePath);
                }

                Global.Resolver.ResolveAsset(context, relativePath, extension);
            }
            catch (ThreadAbortException abortException)
            {
                #if DEBUG
                Global.Logger.Debug("Asset processing thread aborted", abortException);
                #endif
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error processing asset", exception);
            }
            context.NotFound();
        }
    }
}
