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
using System.Xml;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Configuration;
using Lotus.Foundation.Extensions.Serialization;
using Lotus.Foundation.Extensions.SitecoreExtensions;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Extensions
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
                var nodes = Factory.GetConfigNode("/sitecore/lotus.extensions");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.extensions config node! Missing or outdated App_Config/Include/Lotus/Lotus.Foundation.Extensions.config?");
                
                LoadLoggers(nodes.GetChildElement("logging"));
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus extensions", exception, typeof(Global));
                
                Initialized = false;
            }
        }

        private static void LoadLoggers(XmlNode loggingNode)
        {
            LoggerHelper.CreateLoggersFromXml(loggingNode);
        }
    }
}