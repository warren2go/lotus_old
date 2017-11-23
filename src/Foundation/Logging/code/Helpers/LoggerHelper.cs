using System.Linq;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using Sitecore.Configuration;

namespace Lotus.Foundation.Logging.Helpers
{
    public static class LoggerHelper
    {
        public static IAppender CreateAppenderFromNode(XmlNode node)
        {
            return Factory.CreateObject<IAppender>(node["appender"]);
        }
        
        public static ILotusLogger CreateLoggerFromNode(XmlNode node)
        {
            return node != null ? Sitecore.Configuration.Factory.CreateObject<ILotusLogger>(node) : null;
        }
    }
}