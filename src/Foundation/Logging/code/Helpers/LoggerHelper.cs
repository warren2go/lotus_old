using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using log4net.spi;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.StringExtensions;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging.Helpers
{
    public static class LoggerHelper
    {
        public static ILotusLogger DefaultLogger()
        {
            return new DefaultLogger();
        }
        
        public static Dictionary<string, ILotusLogger> LoadLoggersFromXml(XmlNode loggingNode)
        {
            ConfigureLoggerFromNode(XmlUtil.GetChildElement("log4net", loggingNode));
            
            var loggersNode = XmlUtil.GetChildElement("loggers", loggingNode);
            if (loggersNode != null)
            {
                return XmlUtil.GetChildElements("logger", loggersNode).Select(CreateLoggerFromNode).ToDictionary(MapLoggerToName);     
            }
            return new Dictionary<string, ILotusLogger>();
        }
        
        public static string DetermineCallsite(MethodBase mb = null)
        {
            var methodBase = mb ?? new StackFrame(1).GetMethod();
            return "[{0}:{1}] ".FormatWith(methodBase.DeclaringType == null ? "global:" : methodBase.DeclaringType.Name, methodBase.Name);
        }
        
        public static string GenerateLoggerName(string friendlyName, Type type = null)
        {
            //todo: possibly introduce better naming scheme (eg assmebly + friendlyName) - "Lotus.Feature.MailChimp:Logger"
            return friendlyName;
        }
        
        public static string MapLoggerToName(ILotusLogger logger)
        {
            if (string.IsNullOrEmpty(logger.FriendlyName))
                return "default";
            return logger.FriendlyName;
        }

        public static ILotusLogger CreateLoggerFromNode(XmlNode node)
        {
            return CreateLoggerFromNodeFromChild(node, string.Empty);
        }
        
        public static ILotusLogger CreateLoggerFromNodeFromChild(XmlNode node, string elementName)
        {
            ILotusLogger logger;
            if (string.IsNullOrEmpty(elementName))
            {
                logger = Factory.CreateObject<ILotusLogger>(node) ?? DefaultLogger();
                logger.IncludeStacktrace = MainUtil.StringToBool(XmlUtil.GetAttribute("includeStacktrace", node) ?? "false", false);
                logger.FriendlyName = XmlUtil.GetAttribute("name", node) ?? string.Empty;
            }
            else
            {
                logger = Factory.CreateObject<ILotusLogger>(XmlUtil.GetChildElement(elementName, node)) ?? DefaultLogger();
                logger.IncludeStacktrace = MainUtil.StringToBool(XmlUtil.GetAttribute("includeStacktrace", XmlUtil.GetChildElement(elementName, node)) ?? "false", false);
                logger.FriendlyName = XmlUtil.GetAttribute("name", XmlUtil.GetChildElement(elementName, node)) ?? string.Empty;
            }
            return logger;
        }

        /// <summary>
        /// Attempt to merge nodes containing a log4net node into enviroment.
        /// </summary>
        public static void ConfigureLoggersFromNode(IEnumerable<XmlNode> nodes)
        {
            foreach (var loggerNode in nodes)
            {
                DOMConfigurator.Configure(XmlUtil.GetChildElement("log4net", loggerNode) as XmlElement);
            }
        }
        
        /// <summary>
        /// Attempt to merge log4net node into enviroment.
        /// </summary>
        public static void ConfigureLoggerFromNode(XmlNode node)
        {
            DOMConfigurator.Configure(node as XmlElement);
        }
        
        /// <summary>
        /// Attempt to merge log4net node into enviroment using specified repository.
        /// </summary>
        public static void ConfigureLoggerFromNode(ILoggerRepository repository, XmlNode node)
        {
            DOMConfigurator.Configure(repository, node as XmlElement);
        }
    }
}