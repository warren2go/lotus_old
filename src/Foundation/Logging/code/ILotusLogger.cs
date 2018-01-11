using System;
using System.Xml;
using log4net;

namespace Lotus.Foundation.Logging
{
    public interface ILotusLogger
    {
        bool IncludeStacktrace { get; set; }
        string FriendlyName { get; set; }
        
        string FormatMessage(string message);
        
        ILog GetInnerLogger();
        void Debug(string message, Exception exception = null);
        void Info(string message, Exception exception = null);
        void Warn(string message, Exception exception = null);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
    }
}