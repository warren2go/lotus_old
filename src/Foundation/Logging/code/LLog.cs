using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Lotus.Foundation.Logging.Helpers;

namespace Lotus.Foundation.Logging
{
    public static class LLog
    {
        public static void WriteToFile(string fileName, string message, Exception exception = null, eLotusLogType type = eLotusLogType.INFO, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!Path.HasExtension(fileName))
                fileName += ".txt";
            var logger = LotusLogManager.GetLoggerByFileName(fileName);
            if (logger != null)
            {
                logger.Write(type, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
        
        public static void Debug(string friendlyName, string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly(), friendlyName);
            if (logger != null)
            {
                logger.Write(eLotusLogType.DEBUG, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        public static void Debug(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly());
            if (logger != null)
            {
                logger.Write(eLotusLogType.DEBUG, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        public static void Info(string friendlyName, string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly(), friendlyName);
            if (logger != null)
            {
                logger.Write(eLotusLogType.INFO, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
        
        public static void Info(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly());
            if (logger != null)
            {
                logger.Write(eLotusLogType.INFO, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        public static void Warn(string friendlyName, string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly(), friendlyName);
            if (logger != null)
            {
                logger.Write(eLotusLogType.WARN, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        public static void Warn(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly());
            if (logger != null)
            {
                logger.Write(eLotusLogType.WARN, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
        
        public static void Error(string friendlyName, string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly(), friendlyName);
            if (logger != null)
            {
                logger.Write(eLotusLogType.ERROR, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        public static void Error(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly());
            if (logger != null)
            {
                logger.Write(eLotusLogType.ERROR, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
        
        public static void Fatal(string friendlyName, string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly(), friendlyName);
            if (logger != null)
            {
                logger.Write(eLotusLogType.FATAL, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
        
        public static void Fatal(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var logger = LotusLogManager.GetLogger(Assembly.GetCallingAssembly());
            if (logger != null)
            {
                logger.Write(eLotusLogType.FATAL, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
        }
    }
}