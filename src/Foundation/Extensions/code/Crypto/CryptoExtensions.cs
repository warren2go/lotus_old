using System;
using System.Text;

namespace Lotus.Foundation.Extensions.Crypto
{
    public static class CryptoExtensions
    {
        public static string ToMD5(this string @string, bool toLower = true)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(@string))).Replace("-", string.Empty);
                return toLower ? hash.ToLower() : hash;
            }
        }
    }
}