using System;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Logging
{
    public class DefaultLogger : ILotusLogger
    {
        public string Prefix { get; set; }
        public string Postfix { get; set; }

        protected virtual string FormatMessage(string message)
        {
            return (Prefix ?? string.Empty) + message + (Postfix ?? string.Empty);
        }
        
        public virtual void Debug(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated]" + message, exception);
            }
            else
            {
                Log.Debug(FormatMessage(message), typeof(DefaultLogger));   
            }
        }

        public virtual void Info(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated]" + message, exception);
            }
            else
            {
                Log.Info(FormatMessage(message), typeof(DefaultLogger));   
            }
        }

        public virtual void Warn(string message, Exception exception = null)
        {
            Log.Warn(FormatMessage(message), exception, typeof(DefaultLogger));
        }

        public virtual void Error(string message, Exception exception = null)
        {
            Log.Error(FormatMessage(message), exception, typeof(DefaultLogger));
        }

        public virtual void Fatal(string message, Exception exception = null)
        {
            Log.Fatal(FormatMessage(message), exception, typeof(DefaultLogger));
        }
    }
}