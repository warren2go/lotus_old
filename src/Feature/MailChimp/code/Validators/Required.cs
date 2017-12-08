using Lotus.Feature.MailChimp.Lists;
using Lotus.Foundation.Extensions.RegularExpression;

namespace Lotus.Feature.MailChimp.Validators
{
    public class Required : IMailChimpValidator
    {
        public string Key { get; set; }
        public string Raw { get; set; }

        public Required()
        {
            Key = "required";
        }

        public bool Validate(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}