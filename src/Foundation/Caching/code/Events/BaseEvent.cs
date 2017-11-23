namespace Lotus.Foundation.Caching.Events
{
    public class BaseEvent
    {
        protected bool CanProceed()
        {
            return EventsManager.CanProceed(this);
        }
    }
}