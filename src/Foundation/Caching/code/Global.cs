using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Caching.Configuration;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Caching
{
    internal static class Global
    {
        [NotNull]
        internal static ILotusLogger Logger
        {
            get
            {
                return LoggerContext.Logger;
            }
        }
        
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.caching");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.caching config node! Missing or outdated App_Config/Include/Lotus/Lotus.Foundation.Caching.config?");
                
                LoadLoggers(nodes.GetChildElement("logging"));

                Initialized = true;
            }
            catch (Exception exception)
            {
                LLog.Error("Error initializing lotus caching", exception);

                Initialized = false;
            }
        }
        
        private static void LoadLoggers(XmlNode loggingNode)
        {
            LoggerHelper.CreateLoggersFromXml(loggingNode);
        }
    }
}