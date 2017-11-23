using System;
using Sitecore.Diagnostics;

namespace Lotus.Foundation.Logging
{
    public class DefaultLogger : ILotusLogger
    {
        public void Debug(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated]" + message, exception);
            }
            else
            {
                Log.Debug(message, typeof(DefaultLogger));   
            }
        }

        public void Info(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Warn("[Escalated]" + message, exception);
            }
            else
            {
                Log.Info(message, typeof(DefaultLogger));   
            }
        }

        public void Warn(string message, Exception exception = null)
        {
            Log.Warn(message, exception, typeof(DefaultLogger));
        }

        public void Error(string message, Exception exception = null)
        {
            Log.Error(message, exception, typeof(DefaultLogger));
        }

        public void Fatal(string message, Exception exception = null)
        {
            Log.Fatal(message, exception, typeof(DefaultLogger));
        }
    }
}