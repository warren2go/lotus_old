using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Extensions.Configuration
{
    public sealed class ExtensionsLogger : DefaultLogger
    {
        public ExtensionsLogger()
            : base("Lotus.Foundation.Extensions.Logger")
        {
            Prefix = "[ExtensionsLogger] ";
        }
    }
}