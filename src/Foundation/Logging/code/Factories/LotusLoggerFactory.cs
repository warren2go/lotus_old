using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.spi;
using Lotus.Foundation.Logging.Appenders;
using Lotus.Foundation.Logging.Helpers;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging.Factories
{
    internal static class LotusLoggerFactory
    {
        private static bool _createMode = false;
        private static bool _debugMode = false;
        private static SafeDictionary<string, ILotusLogger> _Loggers = new SafeDictionary<string, ILotusLogger>();

        internal static void Initialize(XmlNode factoryNode)
        {
            bool debug;
            if (bool.TryParse(XmlUtil.GetAttribute("debug", factoryNode), out debug))
            {
                _debugMode = debug;
            }
            
            bool create;
            if (bool.TryParse(XmlUtil.GetAttribute("create", factoryNode), out create))
            {
                _createMode = create;
            }
        }
        
        internal static void Add(string loggerName, ILotusLogger logger)
        {
            if (_Loggers.ContainsKey(loggerName))
            {
                var error = "Duplicate logger detected in LogFactory, will ignore - " + (logger.GetType().DeclaringType ?? logger.GetType()).FullName;
                Log.Error(error, typeof(object));
                InternalLogger.LogToDebugConsole(error);
                if (_debugMode)
                {
                    var stacktrace = Environment.StackTrace;
                    Log.Error(stacktrace, typeof(object));
                    InternalLogger.LogToDebugConsole(stacktrace);
                }
            }
            _Loggers[loggerName] = logger;
        }

        internal static ILotusLogger Get(string loggerName)
        {
            var logger = _Loggers[loggerName];
            if (logger == null)
            {
                if (_createMode)
                {
                    var error = "Logger not found in LogFactory with LoggerName, will create = {0}".FormatWith(loggerName);
                    Log.Error(error, typeof(object));
                    InternalLogger.LogToDebugConsole(error);   
                }
                else
                {
                    var error = "Logger not found in LogFactory with LoggerName = {0}".FormatWith(loggerName);
                    Log.Error(error, typeof(object));
                    InternalLogger.LogToDebugConsole(error);                    
                }

                if (_debugMode)
                {
                    var stacktrace = Environment.StackTrace;
                    Log.Error(stacktrace, typeof(object));
                    InternalLogger.LogToDebugConsole(stacktrace);
                }
            }
            return logger;
        }

        internal static ILotusLogger GetByFileName(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            
            var logger = _Loggers[fileName];
            if (logger == null)
            {
                if (_createMode)
                {
                    var info = "Logger not found in LogFactory with FileName, will create = {0}".FormatWith(fileName);
                    Log.Info(info, typeof(object));
                    InternalLogger.LogToDebugConsole(info);   
                }
                else
                {
                    var info = "Logger not found in LogFactory with FileName = {0}".FormatWith(fileName);
                    Log.Info(info, typeof(object));
                    InternalLogger.LogToDebugConsole(info);                    
                }

                if (_debugMode)
                {
                    var stacktrace = Environment.StackTrace;
                    Log.Error(stacktrace, typeof(object));
                    InternalLogger.LogToDebugConsole(stacktrace);
                }

                logger = CreateLogger(fileName);
                Add(fileName, logger);
            }
            return logger;
        }

        private static ILotusLogger CreateLogger(string loggerName, IAppender appender = default(IAppender))
        {
            var logger = new LotusLogger(loggerName, "false");
            var innerLogger = logger.InnerLogger.Logger as Logger;
            if (innerLogger != null)
            {
                innerLogger.Level = Level.ALL;
                innerLogger.Additivity = true;
                innerLogger.AddAppender(appender ?? CreateAppender(loggerName));
            }
            return logger;
        }

        private static IAppender CreateAppender(string loggerName)
        {
            var appender = new LotusLogFileAppender()
            {
                AppendToFile = true,
                Encoding = Encoding.UTF8,
                File = "$(dataFolder)/logs/" + loggerName + ".{date}.txt",
                Layout = new PatternLayout("%4t %d{ABSOLUTE} %-5p %m%n")
            };
            appender.ActivateOptions();
            return appender;
        }
    }
}