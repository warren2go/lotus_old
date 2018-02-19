namespace Lotus.Foundation.Logging.Structures.SMTP
{
    public class EmailAddress : IEmailAddress
    {
        public string Address { get; set; }
        public IMailingList MailingList { get; set; }
        
        public EmailAddress(string address, string mailinglist = "")
        {
            Address = address;

            if (string.IsNullOrEmpty(mailinglist))
            {
                mailinglist = "";
            }
        }

        public override string ToString()
        {
            return "{0} in {1}".FormatWith(Address, MailingList);
        }
    }
}