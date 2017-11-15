using System;
using log4net;

namespace Lotus.Foundation.Assets.Configuration
{
    internal static class AssetsLogger
    {
        private static string AssetsLoggerName = "Lotus.Foundation.Assets.Logger";

        private static readonly ILog _logger = LogManager.GetLogger(AssetsLoggerName);
        
        public static void Debug(string message)
        {
            _logger.Info("[Debug] " + message);
        }

        public static void Info(string message)
        {
            _logger.Info("[Info] " + message);
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