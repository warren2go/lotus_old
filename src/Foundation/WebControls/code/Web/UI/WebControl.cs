using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Diagnostics.PerformanceCounters;

namespace Lotus.Foundation.WebControls.Web.UI
{
    public abstract class WebControl : Sitecore.Web.UI.WebControl
    {
        private bool _usedCache { get; set; }
        
        public override string GetCacheKey()
        {
            return base.GetCacheKey();
        }

        protected override void AddToCache(string cacheKey, string html)
        {
            base.AddToCache(cacheKey, html);
        }
        
        protected new string GetCachedHtml(string cacheKey)
        {
            return base.GetCachedHtml(cacheKey);
        }

        protected override string GetCachingID()
        {
            return base.GetCachingID();
        }
        
        protected new bool RenderFromCache(string cacheKey, HtmlTextWriter output)
        {
            Error.AssertString(cacheKey, nameof (cacheKey), true);
            Error.AssertObject((object) output, nameof (output));
            var cachedHtml = this.GetCachedHtml(cacheKey);
            if (string.IsNullOrEmpty(cachedHtml))
                return false;
            output.Write(cachedHtml);
            this._usedCache = true;
            RenderingCounters.ControlsRenderedFromCache.Increment();
            return true;
        }
    }
}