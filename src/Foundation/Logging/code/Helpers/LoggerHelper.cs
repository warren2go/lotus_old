using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using log4net.spi;
using Lotus.Foundation.Logging.Factories;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging.Helpers
{
    public static class LoggerHelper
    {
        private static readonly ILotusLogger _LogLogger = new LotusLogger(Constants._LOTUSLOGGERID, "false");
        
        public static ILotusLogger FallbackLogger
        {
            get
            {
                return _LogLogger;
            }
        }

        public static string DetermineCallsites(int position, int height)
        {
            var sb = new StringBuilder("Callsites[{0}+{1}]:".FormatWith(position, height) + Environment.NewLine);
            for (var i = 0; i < height; i++)
            {
                var mb = new StackFrame(position + height).GetMethod();
                sb.Append(DetermineCallsite(mb, "\u21b3 {0}{1}" + Environment.NewLine));
            }
            return sb.ToString();
        }

        public static string DetermineCallsite(int position)
        {
            try
            {
                return DetermineCallsite(new StackFrame(position).GetMethod());
            }
            catch (Exception exception)
            {
                InternalLogger.LogToDebugConsole("LoggerHelper->DetermineCallsite : Error determining callsite with position = {0}".FormatWith(exception.Message));
                return "[undefined.unknown()] ";
            }
        }

        public static string DetermineCallsite(MethodBase mb = null, string format = "[{0}{1}] ")
        {
            try
            {
                var methodBase = mb ?? new StackFrame(1).GetMethod();
                return format.FormatWith(methodBase.DeclaringType == null ? "global::" : "{0}.".FormatWith(methodBase.DeclaringType.FullName), DetermineMethodSignature(methodBase));
            }
            catch (Exception exception)
            {
                InternalLogger.LogToDebugConsole("LoggerHelper->DetermineCallsite : Error determining callsite = {0}".FormatWith(exception.Message));
                return "[undefined.unknown()] ";
            }
        }

        public static string DetermineMethodSignature(MethodBase methodBase)
        {
            try
            {
                var sb = new StringBuilder("{0}(".FormatWith(methodBase.Name));
                var parameters = methodBase.GetParameters();
                foreach (var param in parameters)
                {
                    sb.Append(param.ParameterType.Name + " " + param.Name);
                    sb.Append(", ");
                }
                if (parameters.Length > 0)
                    return sb.ToString(0, sb.Length - 2) + ")";
                return sb.ToString() + ")";
            }
            catch (Exception exception)
            {
                InternalLogger.LogToDebugConsole("LoggerHelper->DetermineMethodSignature : Error determining method signature = {0}".FormatWith(exception.Message));
                return "unknown()";
            }
        }
        
        public static string GenerateLoggerName(Assembly callingAssembly, string friendlyName = null)
        {
            return GenerateLoggerName(callingAssembly.GetName(), friendlyName);
        }

        public static string GenerateLoggerName(AssemblyName callingAssemblyName, string friendlyName = null)
        {
            var name = callingAssemblyName.Name;
            if (string.IsNullOrEmpty(friendlyName)) friendlyName = "Logger";
            return string.IsNullOrEmpty(name) ? "Lotus.Foundation.Logging.{0}".FormatWith(friendlyName) : "{0}.{1}".FormatWith(name, friendlyName);
        }
        
        private static string MapLoggerToName(ILotusLogger logger, Assembly assembly = null)
        {
            return GenerateLoggerName(assembly ?? Assembly.GetAssembly(logger.GetType()), logger.FriendlyName);
        }
        
        public static void CreateLoggersFromXml(XmlNode loggingNode, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();
            
            ConfigureLoggerFromXml(XmlUtil.GetChildElement("log4net", loggingNode));
            
            var loggersNode = XmlUtil.GetChildElement("loggers", loggingNode);
            if (loggersNode != null)
            {
                XmlUtil.GetChildElements("logger", loggersNode).Each(x => CreateLoggerFromXml(x, assembly));
            }
        }

        public static void CreateLoggerFromXml(XmlNode node, Assembly callingAssembly)
        {
            CreateLoggerFromXml(node, string.Empty, callingAssembly);
        }
        
        public static void CreateLoggerFromXml(XmlNode node, string elementName, Assembly callingAssembly)
        {
            ILotusLogger logger;
            if (string.IsNullOrEmpty(elementName))
            {
                logger = CreateLogger(node) ?? FallbackLogger;

                if (!logger.IncludeStacktrace)
                {
                    logger.IncludeStacktrace = MainUtil.StringToBool(XmlUtil.GetAttribute("includeStacktrace", node) ?? "false", false);
                }
                logger.FriendlyName = XmlUtil.GetAttribute("friendlyName", node);
            }
            else
            {
                logger = CreateLogger(XmlUtil.GetChildElement(elementName, node)) ?? FallbackLogger;
                
                if (!logger.IncludeStacktrace)
                {
                    logger.IncludeStacktrace = MainUtil.StringToBool(XmlUtil.GetAttribute("includeStacktrace", XmlUtil.GetChildElement(elementName, node)) ?? "false", false);   
                }
                logger.FriendlyName = XmlUtil.GetAttribute("friendlyName", XmlUtil.GetChildElement(elementName, node));
            }
            LotusLoggerFactory.Add(MapLoggerToName(logger, callingAssembly), logger);
        }

        private static ILotusLogger CreateLogger(XmlNode node, bool assert = false)
        {
            try
            {
                return Factory.CreateObject<ILotusLogger>(node);
            }
            catch (Exception exception)
            {
                var error = "Error creating logger from XML node [{0}] = {1}".FormatWith(node.LocalName, node.InnerXml);
                Log.Error(error, exception, typeof(object));
                InternalLogger.LogToDebugConsole(error);
                if (assert) { throw; }
                return null;
            }
        }

        /// <summary>
        /// Attempt to merge nodes containing a log4net node into enviroment.
        /// </summary>
        public static void ConfigureLoggersFromXml(IEnumerable<XmlNode> nodes)
        {
            foreach (var loggerNode in nodes)
            {
                ConfigureLoggerFromXml(loggerNode);
            }
        }
        
        /// <summary>
        /// Attempt to merge log4net node into enviroment.
        /// </summary>
        public static void ConfigureLoggerFromXml(XmlNode node)
        {
            if (node == null) return;
            DOMConfigurator.Configure(node as XmlElement);
        }
        
        /// <summary>
        /// Attempt to merge log4net node into enviroment using specified repository.
        /// </summary>
        public static void ConfigureLoggerFromXml(ILoggerRepository repository, XmlNode node)
        {
            if (node == null) return;
            DOMConfigurator.Configure(repository, node as XmlElement);
        }
    }
}