using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Configuration;
using Lotus.Foundation.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.WebControls
{
    internal static class Global
    {
        internal static ILotusLogger Logger { get; set; }
        internal static Dictionary<string, ILotusLogger> Loggers { get; set; }
        
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.webcontrols");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.extensions config node! Missing or outdated App_Config/Include/Foundation/Foundation.WebControls.config?");
                
                LoadLoggers(nodes.GetChildElement("logging"));
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus webcontrols", exception, typeof(Global));
                
                Initialized = false;
            }
        }
        
        public static ILotusLogger GetLogger(string friendlyName = null, Type type = null)
        {
            return Loggers.TryGetValueOrDefault(LoggerHelper.GenerateLoggerName(friendlyName, type ?? typeof(DefaultLogger)));
        }

        private static void LoadLoggers(XmlNode loggingNode)
        {
            Loggers = LoggerHelper.LoadLoggersFromXml(loggingNode);
            Logger = Loggers.Values.FirstOrDefault(x => string.IsNullOrEmpty(x.FriendlyName) || x.FriendlyName == "default") ?? LoggerHelper.DefaultLogger();
        }
    }
}