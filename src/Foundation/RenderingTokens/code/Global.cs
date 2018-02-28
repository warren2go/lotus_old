using System;
using System.Xml;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace Lotus.Foundation.RenderingTokens
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
                var nodes = Factory.GetConfigNode("/sitecore/lotus.renderingtokens");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.renderingtokens config node! Missing or outdated App_Config/Include/Lotus/Lotus.Foundation.RenderingTokens.config?");
                
                LoadLoggers(XmlUtil.GetChildElement("logging", nodes));
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus renderingtokens", exception, typeof(Exception));
            }
        }
        
        private static void LoadLoggers(XmlNode loggingNode)
        {
            LoggerHelper.CreateLoggersFromXml(loggingNode);
        }
    }
}