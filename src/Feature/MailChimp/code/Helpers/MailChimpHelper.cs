using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Feature.MailChimp.Helpers
{
    public static class MailChimpHelper
    {
        public static string FilterDateTime(string date, string @default = "01/01/1900")
        {
            return string.IsNullOrEmpty(date) ? @default : date.ReplacePattern(@"[/\- ]", @"/");
        }
    }
}