using System;

namespace Lotus.Foundation.Extensions.Date
{
    public static class DateExtensions
    {
        private static readonly DateTime StaticDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static int ToUnixTimestamp(this DateTime date, bool universal = true)
        {
            return (int)((universal ? date : date.ToUniversalTime()) - StaticDateTime).TotalSeconds;
        }
    }
}