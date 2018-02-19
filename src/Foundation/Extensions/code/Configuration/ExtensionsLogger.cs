using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Extensions.Configuration
{
    public sealed class ExtensionsLogger : LotusLogger
    {
        public ExtensionsLogger(string id, string includeStacktrace = "false")
            : base(id, includeStacktrace)
        {
            
        }
    }
}