using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Logging;
using Sitecore;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.WebControls.Configuration
{
    public sealed class WebControlsLogger : DefaultLogger
    {
        public WebControlsLogger()
            : base("Lotus.Foundation.WebControls.Logger")
        {
            Prefix = "[WebControlsLogger] ";
        }
    }
}