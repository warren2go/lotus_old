using Lotus.Foundation.Kernel.Structures.Collections;

namespace Lotus.Foundation.Logging.Structures.SMTP
{
    public class MailingList : IMailingList
    {
        private StaticList<IEmailAddress> Addresses = new StaticList<IEmailAddress>();
        private StaticDictionary<IMailingList, IEmailAddress> AddressesByMailingList = new StaticDictionary<IMailingList, IEmailAddress>();
        
        public void AddAddress(IEmailAddress address)
        {
            Addresses.Add(address);
            AddressesByMailingList[address.MailingList] = address;
        }
    }
}