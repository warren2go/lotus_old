using System;

namespace Lotus.Foundation.Logging
{
    internal static class InternalLogger
    {
        internal static void LogToDebugConsole(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
        
        internal static void LogToDebugConsole(char[] buffer, int index, int count) {
            System.Diagnostics.Debug.Write(new string(buffer, index, count));
        }
    }
}