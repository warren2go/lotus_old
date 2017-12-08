using Lotus.Feature.MailChimp.Lists;
using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Feature.MailChimp.Validators
{
    public class Regex : IMailChimpValidator
    {
        public string Key { get; set; }
        public string Raw { get; set; }

        public Regex(string regex)
        {
            Raw = regex;
        }

        public bool Validate(string value)
        {
            return value.IsMatch(Raw);
        }
    }
}