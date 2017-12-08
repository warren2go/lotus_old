using System.Collections.Generic;

namespace Lotus.Feature.MailChimp.Lists
{
    public interface IMailChimpSubscriber
    {
        IDictionary<string, object> Fields { get; set; }
        void Add<T>(string key, T value);
        object Get(string key);
        T GetAndCast<T>(string key);
        bool Validate(IMailChimpMergeVar mergeVar);
    }
}