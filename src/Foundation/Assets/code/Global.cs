using System;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Assets.Configuration;
using Lotus.Foundation.Assets.Paths;
using Lotus.Foundation.Assets.Resolvers;
using Lotus.Foundation.Assets.Repository;
using Sitecore.Configuration;

namespace Lotus.Foundation.Assets
{
    internal static class Global
    {
        internal static IAssetResolver Resolver;
        internal static IAssetRepository Repository;

        internal static bool Initialized
        {
            get { return Repository != null && Resolver != null; }
        }

        internal static void Initialize()
        {
            try
            {
                var lotusAssets = Factory.GetConfigNode("/sitecore/lotus.assets");
                Sitecore.Diagnostics.Assert.IsNotNull((object) lotusAssets,
                    "Missing lotus.assets config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");

                var resolverNode = lotusAssets["resolver"];
                Sitecore.Diagnostics.Assert.IsNotNull((object) resolverNode,
                    "Missing lotus.assets.resolver config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");

                var repositoryNode = lotusAssets["repository"];
                Sitecore.Diagnostics.Assert.IsNotNull((object) repositoryNode,
                    "Missing lotus.assets.repository config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");
                
                Resolver = Factory.CreateObject<IAssetResolver>(resolverNode);
                Repository = Factory.CreateObject<IAssetRepository>(repositoryNode);
            }
            catch (Exception exception)
            {
                AssetsLogger.Error("Error initializing lotus assets", exception);

                Resolver = null;
                Repository = null;
            }
        }
    }
}
