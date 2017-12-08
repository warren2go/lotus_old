﻿/*
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
using Lotus.Foundation.Assets.Handlers;
using Lotus.Foundation.Assets.Pipelines;
using Lotus.Foundation.Assets.Resolvers;
using Lotus.Foundation.Assets.Repository;
using Lotus.Foundation.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Assets
{
    internal static class Global
    {
        internal static IAssetResolver Resolver { get; set; }
        internal static IAssetRepository Repository { get; set; }
        internal static IEnumerable<IAssetPipeline> Pipelines { get; set; }
        internal static ILotusLogger Logger { get; set; }

        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Factory.GetConfigNode("/sitecore/lotus.assets");
                Sitecore.Diagnostics.Assert.IsNotNull((object) nodes,
                    "Missing lotus.assets config node! Missing or outdated App_Config/Include/Foundation/Foundation.Assets.config?");

//                var sitecore = ConfigurationManager.GetSection("sitecore");
//                if (sitecore != null)
//                {
//                    var variableNodes = GetNodes(".//sc.variable", sitecore);
//                }
                
                //DOMConfigurator.Configure(nodes.ByElementName("log4net") as XmlElement);
                
                Logger = LoggerHelper.CreateLoggerFromNode(nodes.GetChildElement("logger"));
                
                LoadResolver(nodes.GetChildElement("resolver"));
                LoadRepository(nodes.GetChildElement("repository"));
                LoadPipelines(nodes.GetChildElement("pipelines"));

                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus assets", exception, typeof(AssetsRequestHandler));

                Resolver = null;
                Repository = null;
                Pipelines = null;
            }
        }
        
        private static XmlNodeList GetNodes(string xpath, object reader)
        {
            BindingFlags invokeAttr = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField;
            XmlNode xmlNode = reader.GetType().InvokeMember("_section", invokeAttr, (Binder) null, reader, (object[]) null) as XmlNode;
            if (xmlNode != null)
                return xmlNode.SelectNodes(xpath);
            return (XmlNodeList) null;
        }

        private static void LoadResolver(XmlNode resolverNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) resolverNode,
                "Missing lotus.assets.resolver config node! Missing or outdated App_Config/Include/Foundation/Foundation.Assets.config?");
            Resolver = resolverNode.ToObject<IAssetResolver>();
        }

        private static void LoadRepository(XmlNode repositoryNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) repositoryNode,
                "Missing lotus.assets.repository config node! Missing or outdated App_Config/Include/Foundation/Foundation.Assets.config?");
            Repository = repositoryNode.ToObject<IAssetRepository>();
        }

        private static void LoadPipelines(XmlNode pipelinesNode)
        {
            Sitecore.Diagnostics.Assert.IsNotNull((object) pipelinesNode,
                "Missing lotus.assets.pipelines config node! Missing or outdated App_Config/Include/Foundation/Foundation.Assets.config?");
            var requestPipelines = pipelinesNode.GetChildElement("request");
            if (requestPipelines.HasChildNodes)
            {
                Pipelines = requestPipelines.ChildNodes.OfType<XmlElement>().ToObject<IAssetPipeline>();   
            }
        }
    }
}
