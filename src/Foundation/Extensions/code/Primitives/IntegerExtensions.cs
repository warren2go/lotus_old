using System;

namespace Lotus.Foundation.Extensions.Primitives
{
    public static class IntegerExtensions
    {        
        public static string ToHex(this int number, bool strip = false)
        {
            var hex = number.ToString("X2");
            if (strip)
            {
                hex = hex.Replace("0x", "");
            }
            return hex;
        }
    }
}