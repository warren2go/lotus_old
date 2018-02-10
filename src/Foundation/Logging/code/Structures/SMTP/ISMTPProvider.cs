namespace Lotus.Foundation.Logging.Structures.SMTP
{
    public interface ISMTPProvider
    {
        void Send(IEmail email, IMailingList mailinglist, bool assert = true);
    }
}