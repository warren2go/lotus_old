using System.Reflection;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Feature.MailChimp.Configuration
{
    public sealed class MailChimpLogger : LotusLogger
    {        
        public MailChimpLogger(string id, string includeStacktrace = "false")
            : base(id, includeStacktrace)
        {
            
        }
    }
}