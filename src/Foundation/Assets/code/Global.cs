using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Foundation.Assets.Handlers;
using Lotus.Foundation.Assets.Pipelines;
using Lotus.Foundation.Assets.Resolvers;
using Lotus.Foundation.Assets.Repository;
using Lotus.Foundation.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Assets
{
    internal static class Global
    {
        internal static IAssetResolver Resolver;
        internal static IAssetRepository Repository;
        internal static IEnumerable<IAssetPipeline> Pipelines;
        internal static ILotusLogger Logger;

        internal static bool Initialized { get; set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.assets");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.assets config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");

                LoadResolver(nodes.ByElementName("resolver"));
                LoadRepository(nodes.ByElementName("repository"));
                LoadPipelines(nodes.ByElementName("pipelines"));
                LoadLogger(nodes.ByElementName("logger"));

                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus assets", exception, typeof(AssetsRequestHandler));

                Resolver = null;
                Repository = null;
                Pipelines = null;
                Logger = null;
            }
        }

        private static void LoadResolver(XmlNode resolverNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) resolverNode,
                "Missing lotus.assets.resolver config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");
            Resolver = resolverNode.ToObject<IAssetResolver>();
        }

        private static void LoadRepository(XmlNode repositoryNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) repositoryNode,
                "Missing lotus.assets.repository config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");
            Repository = repositoryNode.ToObject<IAssetRepository>();
        }

        private static void LoadPipelines(XmlNode pipelinesNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) pipelinesNode,
                "Missing lotus.assets.pipelines config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");
            var requestPipelines = pipelinesNode.ByElementName("request");
            if (requestPipelines.HasChildNodes)
            {
                Pipelines = requestPipelines.ChildNodes.OfType<XmlElement>().ToObject<IAssetPipeline>();   
            }
        }

        private static void LoadLogger(XmlNode loggerNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) loggerNode,
                "Missing lotus.assets.logger config node! Missing or outdated App_Config/Include/Lotus.Foundation.Assets.config?");
            Logger = loggerNode.ToObject<ILotusLogger>();
        }
    }
}
