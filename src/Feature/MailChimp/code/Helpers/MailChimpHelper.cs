using System;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;

namespace Lotus.Feature.MailChimp.Helpers
{
    public static class MailChimpHelper
    {
        public static string FormatIsoDate(string isoDate, string format = "dd/MM/yyyy")
        {
            return Sitecore.DateUtil.IsoDateToDateTime(isoDate, DateTime.MinValue).ToString(format);
        }
    }
}