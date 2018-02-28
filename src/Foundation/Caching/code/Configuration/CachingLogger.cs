using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Caching.Configuration
{
    public sealed class CachingLogger : LotusLogger
    {
        public CachingLogger(string id, string includeStacktrace = "false")
            : base(id, includeStacktrace)
        {
            
        }
    }
}