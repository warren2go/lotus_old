using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Sitecore;

namespace Lotus.Foundation.Kernel.Utils
{
    public static class StringUtil
    {
        private static readonly string[] _timeDateElements = new string[6] { "y|", "M", "d|86400", "h|3600", "m|60", "s|1" };
        private const string _timeDateStringRegex = @"^(?:\d[yMdhms])+$";
        private const string _timeDateStringElementRegex = @"^(\d[yMdhms])+$";

        public static int ConvertToSeconds(string @string)
        {
            //todo: calculate the total seconds from a string time range
            // example: 14d7h3m would become (14 * (60 * 60 * 24)) + (60 * 60 * 7) + (60 * 3)
            return 0;
        }
        
        /// <summary>
        /// Get the total amount of minutes from a string time range
        /// </summary>
        public static int ConvertToMinutes(string @string)
        {
            //todo: calculate the total minutes from a string time range
            // example: 14d7h3m would become (14 * (60 * 24)) + (60 * 7) + 3
            return 0;
        }

        /// <summary>
        /// Get the total amount of hours from a string time range
        /// </summary>
        public static int ConvertToHours(string @string)
        {
            //todo: calculate the total hours from a string time range
            // example: 14d7h3m would become (14 * 24) + 7 + (1 / 3)
            return 0;
        }

        /// <summary>
        /// Get the total amount of days from a string time range
        /// </summary>
        public static int ConvertToDays(string @string)
        {
            //todo: calculate the total days from a string time range
            // example: 14d7h3m would become 14 + (1 / 7) + (1 / (1 / 3))
            return 0;
        }
        
        /// <summary>
        /// Convert a string time range into a unix date using today as the start
        /// </summary>
        public static long ConvertToUnixFromToday(string @string)
        {
            if (@string.IsMatch(_timeDateStringRegex))
            {
                var result = 0L;
                var elements = @string.ExtractPatterns(_timeDateStringElementRegex);
                foreach (var element in elements)
                {
                    for (var index = 0; index < _timeDateElements.Length; index++)
                    {
                        if (element.EndsWith(_timeDateElements[index]))
                        {
                            
                        }
                    }
                }
            }
            else
            {
                if (DateUtil.IsIsoDate(@string))
                {
                    //todo: finish this to convert string times into unix
                    // example - "14d7h3m would become today + 14 days 7 hours 3 minutes
                    // convert from ISO
                }
                else
                {
                    // attempt to cast to long
                }
            }

            return 0L;
        }
    }
}