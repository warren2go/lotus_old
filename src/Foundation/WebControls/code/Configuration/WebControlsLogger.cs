using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Logging;
using Sitecore;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.WebControls.Configuration
{
    public sealed class WebControlsLogger : LotusLogger
    {
        public WebControlsLogger(string id, string includeStacktrace = "false")
            : base(id, includeStacktrace)
        {
            Pattern = "[WebControlsLogger] ";
        }
    }
}