using Lotus.Foundation.Logging;

namespace Lotus.Feature.MailChimp.Configuration
{
    public class MailChimpLogger : DefaultLogger
    {
        public MailChimpLogger()
        {
            Prefix = "[MailChimpLogger]";
        }
    }
}