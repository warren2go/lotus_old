using Lotus.Feature.MailChimp.Lists;
using MailChimp.Helper;

namespace Lotus.Feature.MailChimp
{
    public interface IMailChimpManager
    {
        global::MailChimp.MailChimpManager Manager { get; set; }
        IMailChimpList List { get; set; }
        IMailChimpMergeVar MergeVar { get; set; }
        string EmailType { get; set; }
        bool DoubleOptin { get; set; }
        bool UpdateExisting { get; set; }
        bool ReplaceInterests { get; set; }
        bool SendWelcome { get; set; }
        
        bool Subscribe(IMailChimpSubscriber subscriber, string emailField = "email", bool skipValidate = false);
    }
}