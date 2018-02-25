/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 *
 * Copyright:      Warren Dawes (warren2go.com) @ RHM (redhotminute.com.au)
 * Date Created:   19/11/2017
 */

using System;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Kernel
{
    internal static class Global
    {
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.kernel");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.kernel config node! Missing or outdated App_Config/Include/Foundation/Foundation.Kernel.config?");
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus kernel", exception);
            }
        }
    }
}
