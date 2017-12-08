namespace Lotus.Feature.MailChimp.Lists
{
    public interface IMailChimpList
    {
        string APIKey { get; set; }
        string ListId { get; set; }
        IMailChimpMergeVar MergeVar { get; set; }
        
        bool SubscribeToList(params object[] parameters);
    }
}