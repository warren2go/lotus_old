using System.Collections.Generic;

namespace Lotus.Feature.MailChimp.Lists
{
    public interface IMailChimpSubscriber
    {
        IDictionary<string, string> Fields { get; set; }
        void Add(string key, string value);
        string Get(string key);
        T GetAndCast<T>(string key);
        IEnumerable<string> Validate(IMailChimpMergeVar mergeVar);
    }
}