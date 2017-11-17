using System;
using log4net;

namespace Lotus.Foundation.Extensions.Configuration
{
    public class ExtensionsLogger
    {
        private static string ExtensionsLoggerName = "Lotus.Foundation.Extensions.Logger";

        private static readonly ILog _logger = LogManager.GetLogger(ExtensionsLoggerName);
        
        public static void Debug(string message)
        {
            _logger.Info("[Debug] " + message);
        }

        public static void Debug(string message, Exception exception)
        {
            _logger.Info("[Debug Exception] " + message, exception);
        }
        
        public static void Info(string message)
        {
            _logger.Warn("[Info] " + message);
        }
        
        public static void Info(string message, Exception exception)
        {
            _logger.Info("[Info Exception] " + message, exception);
        }

        public static void Warn(string message)
        {
            _logger.Warn("[Warn] " + message);
        }

        public static void Warn(string message, Exception exception)
        {
            _logger.Warn("[Warn Exception] " + message, exception);
        }

        public static void Error(string message)
        {
            _logger.Error("[Error] " + message);
        }

        public static void Error(string message, Exception exception)
        {
            _logger.Error("[Error Exception] " + message, exception);
        }
    }
}