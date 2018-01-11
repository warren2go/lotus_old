using System.Reflection;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Feature.MailChimp.Configuration
{
    public sealed class MailChimpLogger : DefaultLogger
    {        
        public MailChimpLogger()
            : base("Lotus.Feature.MailChimp.Logger")
        {
            Prefix = "[MailChimpLogger] ";
        }
    }
}