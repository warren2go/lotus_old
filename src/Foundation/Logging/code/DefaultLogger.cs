using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.StringExtensions;
using Sitecore.Xml;

namespace Lotus.Foundation.Logging
{
    public class DefaultLogger : ILotusLogger
    {
        private readonly ILog _logger;

        public bool IncludeStacktrace { get; set; }
        public string FriendlyName { get; set; }

        protected string Prefix { get; set; }
        protected string Postfix { get; set; }

        public DefaultLogger()
        {
            _logger = LogManager.GetLogger("Lotus.Foundation.Logging.Logger");
            
            Prefix = "[LoggingLogger] ";
        }
        
        public DefaultLogger(string name, string prefix = null, string postfix = null)
        {
            _logger = LogManager.GetLogger(name ?? "Lotus.Foundation.Logging.Logger");
            
            Prefix = prefix;
            Postfix = postfix;
        }

        public virtual ILog GetInnerLogger()
        {
            return _logger;
        }
        
        public virtual string FormatMessage(string message, string callerMemberName = "", string callerFilePath = "", int callerLineNumber = -1)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("[{0}:{1}]->{2}{3}", callerFilePath, callerLineNumber, callerMemberName, Environment.NewLine));
            sb.Append(LoggerHelper.DetermineCallsite(new StackFrame(2).GetMethod()) + Environment.NewLine);
            sb.Append(Prefix ?? string.Empty);
            sb.Append(message);
            sb.Append(Postfix ?? string.Empty);
            sb.Append(Environment.NewLine);
            
            if (IncludeStacktrace)
            {
                sb.Append(Environment.NewLine + "Stack:" + Environment.NewLine);
                sb.Append(Environment.StackTrace);
                sb.Append(Environment.NewLine);
            }
            
            return sb.ToString();
        }
        
        public virtual void Debug(string message, Exception exception = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (exception != null)
            {
                if (_logger != null)
                {
                    _logger.Warn(FormatMessage("[Escalated from Debug] " + message, callerMemberName, callerFilePath, callerLineNumber));
                }
                else
                {
                    Log.Warn(FormatMessage("[Escalated from Debug] " + message, callerMemberName, callerFilePath, callerLineNumber), typeof(DefaultLogger));   
                }
            }
            else
            {
                if (_logger != null)
                {
                    _logger.Debug(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber));
                }
                else
                {
                    Log.Debug(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), typeof(DefaultLogger));   
                }
            }
        }

        public virtual void Info(string message, Exception exception = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (exception != null)
            {
                if (_logger != null)
                {
                    _logger.Warn(FormatMessage("[Escalated from Info] " + message, callerMemberName, callerFilePath, callerLineNumber));
                }
                else
                {
                    Log.Warn(FormatMessage("[Escalated from Info] " + message, callerMemberName, callerFilePath, callerLineNumber), typeof(DefaultLogger));   
                }
            }
            else
            {
                if (_logger != null)
                {
                    _logger.Info(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber));
                }
                else
                {
                    Log.Info(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), typeof(DefaultLogger));   
                }
            }
        }

        public virtual void Warn(string message, Exception exception = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (_logger != null)
            {
                _logger.Warn(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
            }
            else
            {
                Log.Warn(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, typeof(DefaultLogger));   
            }
        }

        public virtual void Error(string message, Exception exception = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (_logger != null)
            {
                _logger.Error(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
            }
            else
            {
                Log.Error(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception, typeof(DefaultLogger));   
            }
        }

        public virtual void Fatal(string message, Exception exception = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (_logger != null)
            {
                _logger.Fatal(FormatMessage(message, callerMemberName, callerFilePath, callerLineNumber), exception);
            }
            else
            {
                Log.Fatal(FormatMessage(message), exception, typeof(DefaultLogger));   
            }
        }
    }
}