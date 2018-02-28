using System;
using System.Collections;
using System.Linq;
using Lotus.Foundation.Kernel.Extensions.Casting;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.Sites;

namespace Lotus.Foundation.Caching.Events.Publishing
{
    public class HtmlCacheClearer : BaseEvent
    {
        private readonly ArrayList _sites = new ArrayList();

        public ArrayList Sites
        {
            get
            {
                return this._sites;
            }
        }
        
        /// <summary>
        /// Clear cache event - sender is <see cref="Event.EventSubscribers"/> and args is <see cref="SitecoreEventArgs"/>
        /// </summary>
        /// <param name="sender">The invoker of the event.</param>
        /// <param name="args">Arguments supplied to the event.</param>
        public void ClearCache(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, nameof (sender));
            Assert.ArgumentNotNull((object) args, nameof (args));

            try
            {
                var eventArgs = args as SitecoreEventArgs;
                if (eventArgs != null)
                {
                    var publisher = eventArgs.Parameters.FirstOrDefault(x => x is Publisher) as Publisher;
                    if (publisher != null)
                    {
                        LLog.Info("SitecoreEventArgs[{0}]: Mode = {1}, Deep = {2}, RootItem = {3}:{4}".FormatWith(eventArgs.EventName, publisher.Options.Mode, publisher.Options.Deep, publisher.Options.RootItem.ID, publisher.Options.RootItem.Name));   
                    }
                }
            }
            catch (Exception exception)
            {
                LLog.Error("Exception during arg generation", exception);
            }
            
            LLog.Info("HtmlCacheClearer clearing HTML caches for all sites (" + (object) this._sites.Count + ").");
            for (var index = 0; index < this._sites.Count; ++index)
            {
                var siteName = this._sites[index] as string;
                if (siteName != null)
                {
                    var site = Factory.GetSite(siteName);
                    if (site != null)
                    {
                        LLog.Info("Site: {0}".FormatWith(site.Name));
                        var htmlCache = CacheManager.GetHtmlCache(site);
                        if (htmlCache != null)
                            htmlCache.Clear();
                    }
                }
            }
            LLog.Info("HtmlCacheClearer done.");
        }
    }
}