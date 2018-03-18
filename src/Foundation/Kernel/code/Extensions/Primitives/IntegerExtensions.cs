using System;
using System.Linq;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.Primitives
{
    public static class IntegerExtensions
    {
        private static readonly string[] HexStrip = new[] { "0x", " " };

        [NotNull]
        public static string ToHex(this int number, bool strip = true)
        {
            return number.ToHex(strip, HexStrip);
        }
        
        [NotNull]
        public static string ToHex(this int number, bool strip, string[] stripStrings)
        {
            return strip ? HexStrip.Aggregate(number.ToString("X2"), (current, hexStrip) => current.Replace(hexStrip, string.Empty)) : number.ToString("X2");
        }
    }
}