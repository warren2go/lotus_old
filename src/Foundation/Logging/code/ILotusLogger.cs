using System;
using System.Runtime.CompilerServices;
using System.Xml;
using log4net;

namespace Lotus.Foundation.Logging
{
    public interface ILotusLogger
    {
        ILog InnerLogger { get; }
        bool IncludeStacktrace { get; set; }
        
        string FriendlyName { get; set; }
        string Name { get; }
        
        string FormatMessage(string message, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = -1);

        void Write(eLotusLogType type, string message, Exception exception = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = -1);
        void Debug(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);
        void Info(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);
        void Warn(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);
        void Error(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);
        void Fatal(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);
    }
}