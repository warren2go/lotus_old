/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 *
 * Copyright:      Warren Dawes @ RHM (warren2go.com)
 * Date Created:   19/11/2017
 */

using System;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Extensions
{
    public static class Global
    {
        internal static bool Initialized { get; set; }

        internal static void Initialize()
        {
            try
            {
                var lotusExtensions = Factory.GetConfigNode("/sitecore/lotus.extensions");
                Sitecore.Diagnostics.Assert.IsNotNull((object) lotusExtensions,
                    "Missing lotus.extensions config node! Missing or outdated App_Config/Include/Lotus.Foundation.Extensions.config?");

//                lotusExtensions["logger"].OfType<XmlElement>()
//                    .Select(Factory.CreateObject<ILotusLogger>)
//                    .ToArray();
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus extensions", exception, typeof(Global));
                Initialized = false;
            }
        }
    }
}