using System;
using System.Reflection;
using log4net;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Logging;
using Sitecore;

namespace Lotus.Foundation.Assets.Configuration
{
    public sealed class AssetsLogger : DefaultLogger
    {
        public AssetsLogger() 
            : base("Lotus.Foundation.Assets.Logger")
        {
            Prefix = "[AssetsLogger] ";
        }
    }
}