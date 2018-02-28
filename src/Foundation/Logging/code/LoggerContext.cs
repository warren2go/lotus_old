using System.Reflection;
using Lotus.Foundation.Logging.Helpers;
using Sitecore;
using Sitecore.Collections;

namespace Lotus.Foundation.Logging
{
    public static class LoggerContext
    {
        private static readonly SafeDictionary<Assembly, ILotusLogger> _loggersByAssembly = new SafeDictionary<Assembly, ILotusLogger>();

        /// <summary>
        /// Default logger associated by the calling assembly - use <see cref="LotusLogManager"/> to get custom loggers by friendly names
        /// </summary>
        [NotNull]
        public static ILotusLogger Logger
        {
            get
            {
                var callingAssembly = Assembly.GetCallingAssembly();
                var logger = _loggersByAssembly.TryGetValueOrDefault(callingAssembly);
                if (logger == null)
                {
                    logger = LotusLogManager.GetLogger(callingAssembly) ?? LoggerHelper.FallbackLogger;
                    _loggersByAssembly.Add(callingAssembly, logger);
                }
                return logger;
            }
        }
    }
}