using Lotus.Foundation.Caching.Configuration;

namespace Lotus.Foundation.Caching.Events
{
    internal class EventsManager
    {
        internal static bool CanProceed(BaseEvent @event)
        {
            return CachingSettings.Enabled;
        }
    }
}