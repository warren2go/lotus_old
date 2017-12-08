namespace Lotus.Feature.MailChimp.Lists
{
    internal class GenericList : IMailChimpList
    {
        public string APIKey { get; set; }
        public string ListId { get; set; }
        public IMailChimpMergeVar MergeVar { get; set; }

        public GenericList()
        {
            
        }
        
        public GenericList(string listId)
        {
            ListId = listId;
        }

        public bool SubscribeToList(params object[] parameters)
        {
            return true;
        }
    }
}