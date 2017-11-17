namespace Lotus.Foundation.Extensions.String
{
    public static class StringExtensions
    {
        private static readonly string[] Escapable = { @".", @"+", @"*", @"^", @"?",  @"$" };
        
        public static string SurroundsWith(this string @string, string with)
        {
            return string.Format("{0}{1}{2}",with, @string, with);
        }

        public static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

        public static string Escape(this string @string)
        {
            foreach (var escape in Escapable)
            {
                @string = @string.Replace(escape, @"\{0}".FormatWith(escape));
            }
            return @string;
        }
    }
}