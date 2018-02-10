using System;
using System.Reflection;
using Lotus.Foundation.Logging.Factories;
using Lotus.Foundation.Logging.Helpers;

namespace Lotus.Foundation.Logging
{
    public static class LotusLogManager
    {
        public static ILotusLogger GetLogger(string friendlyName, Type type = null)
        {
            return GetLoggerByName(LoggerHelper.GenerateLoggerName(type == null ? Assembly.GetCallingAssembly() : Assembly.GetAssembly(type), friendlyName));
        }
        
        public static ILotusLogger GetLogger(Assembly callingAssembly, string friendlyName = "Logger")
        {
            return GetLoggerByName(LoggerHelper.GenerateLoggerName(callingAssembly, friendlyName));
        }

        public static ILotusLogger GetLogger(AssemblyName callingAssemblyName, string friendlyName = "Logger")
        {
            return GetLoggerByName(LoggerHelper.GenerateLoggerName(callingAssemblyName, friendlyName));
        }

        public static ILotusLogger GetLoggerByName(string loggerName = null)
        {
            return LotusLoggerFactory.Get(loggerName);
        }

        internal static ILotusLogger GetLoggerByFileName(string fileName)
        {
            return LotusLoggerFactory.GetByFileName(fileName);
        }
    }
}