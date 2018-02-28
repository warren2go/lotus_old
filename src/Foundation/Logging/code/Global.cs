/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 *
 * Copyright:      Warren Dawes (warren2go.com) @ RHM (redhotminute.com.au)
 * Date Created:   19/11/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Lotus.Foundation.Logging.Factories;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging
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
                var nodes = Factory.GetConfigNode("/sitecore/lotus.logging");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.logging config node! Missing or outdated App_Config/Include/Lotus/Lotus.Foundation.Logging.config?");

                LotusLoggerFactory.Initialize(XmlUtil.GetChildElement("logfactory", nodes));
                
                LoadLoggers(XmlUtil.GetChildElement("logging", nodes));
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus logging", exception, typeof(Exception));
            }
        }
        
        private static void LoadLoggers(XmlNode loggingNode)
        {
            LoggerHelper.CreateLoggersFromXml(loggingNode);
        }
    }
}