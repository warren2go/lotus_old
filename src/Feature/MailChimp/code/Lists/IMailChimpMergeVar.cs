using System.Collections.Generic;
using System.Xml;
using Lotus.Feature.MailChimp.Validators;

namespace Lotus.Feature.MailChimp.Lists
{
    public interface IMailChimpMergeVar
    {
        IDictionary<string, string> Fields { get; set; }
        IDictionary<string, IList<IMailChimpValidator>> ValidatorsByFieldName { get; set; }
        void MapField(XmlNode variableNode);
    }
}