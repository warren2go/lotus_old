using System;
using System.Diagnostics;
using System.Reflection;
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
        
        public virtual string FormatMessage(string message)
        {
            var sb = new StringBuilder();
            sb.Append(LoggerHelper.DetermineCallsite(new StackFrame(2).GetMethod()) + Environment.NewLine);
            sb.Append(Prefix ?? string.Empty);
            sb.Append(message);
            sb.Append(Postfix ?? string.Empty);

            if (IncludeStacktrace)
            {
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine + "Stack:" + Environment.NewLine);
                sb.Append(Environment.StackTrace);
                sb.Append(Environment.NewLine);
            }
            
            return sb.ToString();
        }
        
        public virtual void Debug(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated] " + message, exception);
            }
            else
            {
                if (_logger != null)
                {
                    _logger.Debug(FormatMessage(message));
                }
                else
                {
                    Log.Debug(FormatMessage(message), typeof(DefaultLogger));   
                }
            }
        }

        public virtual void Info(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated] " + message, exception);
            }
            else
            {
                if (_logger != null)
                {
                    _logger.Info(FormatMessage(message));
                }
                else
                {
                    Log.Info(FormatMessage(message), typeof(DefaultLogger));   
                }
            }
        }

        public virtual void Warn(string message, Exception exception = null)
        {
            if (_logger != null)
            {
                _logger.Warn(FormatMessage(message), exception);
            }
            else
            {
                Log.Warn(FormatMessage(message), exception, typeof(DefaultLogger));   
            }
        }

        public virtual void Error(string message, Exception exception = null)
        {
            if (_logger != null)
            {
                _logger.Error(FormatMessage(message), exception);
            }
            else
            {
                Log.Error(FormatMessage(message), exception, typeof(DefaultLogger));   
            }
        }

        public virtual void Fatal(string message, Exception exception = null)
        {
            if (_logger != null)
            {
                _logger.Fatal(FormatMessage(message), exception);
            }
            else
            {
                Log.Fatal(FormatMessage(message), exception, typeof(DefaultLogger));   
            }
        }
    }
}