using System;
using System.Xml;
using Lotus.Foundation.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Caching
{
    internal static class Global
    {
        internal static ILotusLogger Logger;
        
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.caching");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.caching config node! Missing or outdated App_Config/Include/Foundation/Foundation.Caching.config?");
                
                Logger = LoggerHelper.CreateLoggerFromNode(nodes.ByElementName("logger"));

                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus caching", exception, typeof(Global));

                Initialized = false;
            }
            finally
            {
                if (Logger == null)
                {
                    Logger = new DefaultLogger();
                }
            }
        }
    }
}