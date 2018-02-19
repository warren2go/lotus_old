using System;

namespace Lotus.Foundation.Logging
{
    internal static class InternalLogger
    {
        internal static void LogToDebugConsole(string message, bool includeStacktrace = true)
        {
            System.Diagnostics.Debug.WriteLine(message);
            
            if (includeStacktrace)
            {
                System.Diagnostics.Debug.WriteLine(Environment.StackTrace);    
            }
        }
        
        internal static void LogToDebugConsole(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.Write(new string(buffer, index, count));
        }
    }
}