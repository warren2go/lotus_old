using System;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.Date
{
    public static class DateExtensions
    {
        private static readonly DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        [NotNull]
        public static int ToUnixTimestamp(this DateTime date, bool treatAsUniversal = true)
        {
            return (int)((treatAsUniversal ? date : date.ToUniversalTime()) - UnixDateTime).TotalSeconds;
        }
        
        [NotNull]
        public static string ToIsoDate(this DateTime datetime, bool includeTicks = false)
        {
            return Sitecore.DateUtil.ToIsoDate(datetime, includeTicks);
        }
    }
}