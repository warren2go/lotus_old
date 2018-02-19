namespace Lotus.Foundation.Logging.Structures.SMTP
{
    public interface IEmailAddress
    {
        string Address { get; set; }
        IMailingList MailingList { get; set; }
    }
}