using Lotus.Feature.MailChimp.Lists;

namespace Lotus.Feature.MailChimp.Validators
{
    public interface IMailChimpValidator
    {
        string Key { get; set; }
        string Raw { get; set; }
        bool Validate(string value);
    }
}