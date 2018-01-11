using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using log4net;
using log4net.Config;
using Lotus.Feature.MailChimp.Configuration;
using Lotus.Feature.MailChimp.Lists;
using Lotus.Feature.MailChimp.Services;
using Lotus.Feature.MailChimp.Validators;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.Serialization;
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
        internal static ILotusLogger Logger { get; set; }
        internal static Dictionary<string, ILotusLogger> Loggers { get; set; }
        
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            try
            {
                var nodes = Sitecore.Configuration.Factory.GetConfigNode("/sitecore/lotus.mailchimp");
                Sitecore.Diagnostics.Assert.IsNotNull(nodes,
                    "Missing lotus.mailchimp config node! Missing or outdated App_Config/Include/Feature/Feature.MailChimp.config?");

                LoadLoggers(nodes.GetChildElement("logging"));
                LoadValidators(nodes.GetChildElement("validators"));
                LoadLists(nodes.GetChildElements("lists"));

                Initialized = true;
            }
            catch (Exception exception)
            {
                Log.Error("Error initializing lotus mailchimp", exception, typeof(string));
            }
        }

        public static ILotusLogger GetLogger(string friendlyName = "default", Type type = null)
        {
            return Loggers.TryGetValueOrDefault(LoggerHelper.GenerateLoggerName(friendlyName, type ?? typeof(MailChimpLogger)));
        }

        private static void LoadLoggers(XmlNode loggingNode)
        {
            Loggers = LoggerHelper.LoadLoggersFromXml(loggingNode);
            Logger = Loggers.Values.FirstOrDefault(x => string.IsNullOrEmpty(x.FriendlyName) || x.FriendlyName == "default") ?? LoggerHelper.DefaultLogger();
        }

        private static void LoadValidators(XmlNode validatorsNode)
        {
            Assert.IsNotNull(validatorsNode, "Validators node is not defined! Missing or outdated App_Config/Include/Feature/Feature.MailChimp.config?");

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

        private static void LoadLists(IEnumerable<XmlNode> listsNodes)
        {
            Assert.IsNotNull(listsNodes, "Lists node is not defined! Missing or outdated App_Config/Include/Feature/Feature.MailChimp.config?");

            foreach (var listNodes in listsNodes)
            {
                foreach (var listNode in listNodes.GetChildElements("list"))
                {
                    var list = listNode.ToObject<IMailChimpList>();
                    if (list != null)
                    {
                        list.APIKey = listNodes.GetAttribute("apiKey");
                        MailChimpService.AddListByID(list);   
                    }
                }
            }
        }
    }
}