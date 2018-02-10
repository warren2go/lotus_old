using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Assets.Configuration
{
    public sealed class AssetsLogger : LotusLogger
    {
        public AssetsLogger(string id, string includeStacktrace = "false") 
            : base(id, includeStacktrace)
        {
            
        }
    }
}