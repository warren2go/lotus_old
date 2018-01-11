using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Caching.Configuration
{
    public sealed class CachingLogger : DefaultLogger
    {
        public CachingLogger()
            : base("Lotus.Foundation.Caching.Logger")
        {
            Prefix = "[CachingLogger] ";
        }
    }
}