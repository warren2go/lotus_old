using System;

namespace Lotus.Foundation.Logging
{
    internal static class Constants
    {
        internal const string _LOTUSLOGGERPATTERN = @"[$(name)] $(message)$(newline)$(source)$(newline)$(stacktrace)";
        internal const string _LOTUSLOGGERID = "Lotus.Foundation.Logging.Logger";
    }
}