using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Assets.Structures;
using Sitecore.Configuration;

namespace Lotus.Foundation.Assets
{
    internal class Global
    {
        internal static Repository Repository;

        internal static bool Initialized
        {
            get { return Repository != null; }
        }

        internal static void Initialize()
        {
            var configNode = Factory.GetConfigNode("/sitecore/lotus.assets");
            Sitecore.Diagnostics.Assert.IsNotNull((object) configNode,
                "Missing lotus.assets config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");

            Repository = new Repository(configNode);
        }
    }
}
