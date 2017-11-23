using System;
using log4net;
using log4net.Appender;
using Lotus.Foundation.Logging;

namespace Lotus.Foundation.Extensions.Configuration
{
    internal class ExtensionsLogger : ILotusLogger
    {
        private static string LoggerName = "Lotus.Foundation.Extensions.Logger";

        private readonly ILog _logger = LogManager.GetLogger(LoggerName);
        
        public ExtensionsLogger()
        {
            _logger.Debug("NAME: " + _logger.Logger.Name);
        }
        
        public void Debug(string message, Exception exception = null)
        {
            _logger.Info("[Debug] " + message, exception);
        }
        
        public void Info(string message, Exception exception = null)
        {
            _logger.Info("[Info] " + message, exception);
        }

        public void Warn(string message, Exception exception = null)
        {
            _logger.Warn("[Warn] " + message, exception);
        }

        public void Error(string message, Exception exception = null)
        {
            _logger.Error("[Error] " + message, exception);
        }

        public void Fatal(string message, Exception exception = null)
        {
            _logger.Fatal("[Fatal] " + message, exception);
        }
    }
}