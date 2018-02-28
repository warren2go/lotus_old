using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using log4net.Config;
using Lotus.Feature.MailChimp.Configuration;
using Lotus.Feature.MailChimp.Lists;
using Lotus.Feature.MailChimp.Services;
using Lotus.Feature.MailChimp.Validators;
using Lotus.Foundation.Kernel.Extensions.Casting;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.Serialization;
using Lotus.Foundation.Logging;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Diagnostics.PerformanceCounters;
using Sitecore.Exceptions;
using Sitecore.Reflection;
using Sitecore.Xml;

namespace Lotus.Feature.MailChimp
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
                var nodes = Sitecore.Configuration.Factory.GetConfigNode("/sitecore/lotus.mailchimp");
                Sitecore.Diagnostics.Assert.IsNotNull(nodes,
                    "Missing lotus.mailchimp config node! Missing or outdated App_Config/Include/Lotus/Lotus..MailChimp.config?");

                LoadLoggers(nodes.GetChildElement("logging"));
                LoadValidators(nodes.GetChildElement("validators"));
                LoadAPIs(nodes.GetChildElements("api"));
                
                Initialized = true;
            }
            catch (Exception exception)
            {
                LLog.Error("Error initializing lotus mailchimp", exception);
            }
        }

        private static void LoadLoggers(XmlNode loggingNode)
        {
            LoggerHelper.CreateLoggersFromXml(loggingNode);
        }

        private static void LoadValidators(XmlNode validatorsNode)
        {
            Assert.IsNotNull(validatorsNode, "Validators node is not defined! Missing or outdated App_Config/Include/Lotus/Lotus..MailChimp.config?");

            foreach (var validatorNode in XmlUtil.GetChildElements("validator", validatorsNode))
                
            {
                var key = validatorNode.GetAttribute("key");
                foreach (var alias in key.Split('|'))
                {
                    var validator = validatorNode.ToObject<IMailChimpValidator>();
                    if (validator != null)
                    {
                        validator.Key = alias;
                        MailChimpService.AddValidatorByKey(alias, validator);
                    }   
                }
            }
            
            MailChimpService.CheckValidators();
        }

        private static void LoadAPIs(IEnumerable<XmlNode> apiNodes)
        {
            Assert.IsNotNull(apiNodes, "Lists node is not defined! Missing or outdated App_Config/Include/Lotus/Lotus.Feature.MailChimp.config?");

            foreach (var apiNode in apiNodes)
            {
                var key = apiNode.GetAttribute("key");
                foreach (var listNode in apiNode.GetChildElements("list"))
                {
                    var list = listNode.ToObject<IMailChimpList>();
                    if (list != null)
                    {
                        list.Key = key;
                        if (string.IsNullOrEmpty(list.ListId))
                            list.ListId = listNode.GetAttribute("listid");
                        MailChimpService.AddListByID(list);   
                    }
                }
            }
        }
    }
}