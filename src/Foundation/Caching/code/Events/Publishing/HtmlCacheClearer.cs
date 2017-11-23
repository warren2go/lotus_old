using System;
using System.Collections;
using System.Linq;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Primitives;
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
        
        // sender = Sitecore.Events.Event.EventSubscriber
        // args = Sitecore.Events.SitecoreEventArgs
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
                        Global.Logger.Info("SitecoreEventArgs[{0}]: Mode = {1}, Deep = {2}, RootItem = {3}:{4}".FormatWith(eventArgs.EventName, publisher.Options.Mode, publisher.Options.Deep, publisher.Options.RootItem.ID, publisher.Options.RootItem.Name));   
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("Exception during arg generation", exception, typeof(HtmlCacheClearer));
            }
            
            Log.Info("HtmlCacheClearer clearing HTML caches for all sites (" + (object) this._sites.Count + ").", (object) this);
            for (int index = 0; index < this._sites.Count; ++index)
            {
                string site1 = this._sites[index] as string;
                if (site1 != null)
                {
                    SiteContext site2 = Factory.GetSite(site1);
                    if (site2 != null)
                    {
                        HtmlCache htmlCache = CacheManager.GetHtmlCache(site2);
                        if (htmlCache != null)
                            htmlCache.Clear();
                    }
                }
            }
            Log.Info("HtmlCacheClearer done.", (object) this);
        }
    }
}