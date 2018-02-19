using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Lotus.Foundation.Logging.Appenders;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.StringExtensions;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging
{
    public enum eLotusLogType
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }
    
    public class LotusLogger : ILotusLogger
    {
        private readonly ILog _InnerLogger;
        public ILog InnerLogger
        {
            get { return _InnerLogger; }
        }

        public bool IncludeStacktrace { get; set; }
        public string FriendlyName { get; set; }
        public string Name
        {
            get { return _InnerLogger.Logger.Name; }
        }

        protected string Pattern { get; set; }
        
        public LotusLogger(string id)
        {
            _InnerLogger = log4net.LogManager.GetLogger(id);

            IncludeStacktrace = false;
            Pattern = Constants._LOTUSLOGGERPATTERN;
        }
        
        public LotusLogger(string id, string includeStacktrace = "false")
        {
            _InnerLogger = log4net.LogManager.GetLogger(id);

            IncludeStacktrace = includeStacktrace.ToBool();
            Pattern = Constants._LOTUSLOGGERPATTERN;
        }
        
        public LotusLogger(string id, string includeStacktrace = "false", string pattern = Constants._LOTUSLOGGERPATTERN)
        {
            _InnerLogger = log4net.LogManager.GetLogger(id);

            IncludeStacktrace = includeStacktrace.ToBool();
            Pattern = pattern;
        }
        
        public virtual string FormatMessage(string message, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = -1)
        {
            return Pattern
                .Replace("$(name)", Name)
                .Replace("$(source)", "Source: {0}:{1}.{2}{3}".FormatWith(callerFilePath, callerLineNumber, callerMemberName, Environment.NewLine))
                .Replace("$(callsite)", LoggerHelper.DetermineCallsite(new StackFrame(3).GetMethod()) + Environment.NewLine)
                .Replace("$(message)", message)
                .Replace("$(stacktrace)", IncludeStacktrace ? Environment.StackTrace : "")
                .Replace("$(newline)", Environment.NewLine);
        }

        public void Write(eLotusLogType type, string message, Exception exception = null, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = -1)
        {
            switch (type)
            {
                case eLotusLogType.DEBUG:
                    if (InnerLogger != null && InnerLogger.IsDebugEnabled)
                    {
                        InnerLogger.Debug(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
                    }
                    else
                    {
                        Log.Debug(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), this);
                    }
                    break;
                
                case eLotusLogType.INFO:
                    if (InnerLogger != null && InnerLogger.IsInfoEnabled)
                    {
                        InnerLogger.Info(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
                    }
                    else
                    {
                        Log.Info(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), this);
                    }
                    break;
                
                case eLotusLogType.WARN:
                    if (InnerLogger != null && InnerLogger.IsWarnEnabled)
                    {
                        InnerLogger.Warn(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
                    }
                    else
                    {
                        Log.Warn(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, this);
                    }
                    break;
                    
                case eLotusLogType.FATAL:
                    if (InnerLogger != null && InnerLogger.IsFatalEnabled)
                    {
                        InnerLogger.Fatal(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
                    }
                    else
                    {
                        Log.Fatal(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, this);
                    }
                    break;
                    
                case eLotusLogType.ERROR:
                default:
                    if (InnerLogger != null && InnerLogger.IsErrorEnabled)
                    {
                        InnerLogger.Error(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
                    }
                    else
                    {
                        Log.Error(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, this);
                    }
                    break;
            }
        }
        
        public virtual void Debug(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (exception != null)
            {
                if (InnerLogger != null)
                {
                    Write(eLotusLogType.WARN, "[Escalated from Debug] {0}".FormatWith(message), exception, callerMemberName, callerFilePath, callerLineNumber);
                }
                else
                {
                    Log.Warn(FormatMessage("[Escalated from Debug] {0}".FormatWith(message), callerMemberName, callerFilePath, callerLineNumber), this);   
                }
            }
            else
            {
                if (InnerLogger != null)
                {
                    Write(eLotusLogType.DEBUG, message, null, callerMemberName, callerFilePath, callerLineNumber);
                }
                else
                {
                    Log.Debug(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), this);   
                }
            }
        }

        public virtual void Info(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (exception != null)
            {
                if (InnerLogger != null)
                {
                    Write(eLotusLogType.WARN, "[Escalated from Info] {0}".FormatWith(message), exception, callerMemberName, callerFilePath, callerLineNumber);
                }
                else
                {
                    Log.Warn(FormatMessage("[Escalated from Info] {0}".FormatWith(message), callerMemberName, callerFilePath, callerLineNumber), this);   
                }
            }
            else
            {
                if (InnerLogger != null)
                {
                    Write(eLotusLogType.INFO, message, null, callerMemberName, callerFilePath, callerLineNumber);
                }
                else
                {
                    Log.Info(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), this);   
                }
            }
        }

        public virtual void Warn(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (InnerLogger != null)
            {
                Write(eLotusLogType.WARN, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
            else
            {
                Log.Warn(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, this);   
            }
        }

        public virtual void Error(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (InnerLogger != null)
            {
                Write(eLotusLogType.ERROR, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
            else
            {
                Log.Error(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, this);   
            }
        }

        public virtual void Fatal(string message, Exception exception = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (InnerLogger != null)
            {
                Write(eLotusLogType.FATAL, message, exception, callerMemberName, callerFilePath, callerLineNumber);
            }
            else
            {
                Log.Fatal(FormatMessage(message), exception, this);   
            }
        }
    }
}