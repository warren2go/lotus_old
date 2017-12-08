using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lotus.Feature.MailChimp.Services;
using Lotus.Feature.MailChimp.Validators;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Primitives;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Extensions.Serialization;
using Sitecore.Configuration;

namespace Lotus.Feature.MailChimp.Lists
{
    internal class GenericMergeVar : IMailChimpMergeVar
    {
        public IDictionary<string,string> Fields { get; set; }
        public IDictionary<string, IList<IMailChimpValidator>> ValidatorsByFieldName { get; set; }

        public GenericMergeVar()
        {
            Fields = new Dictionary<string, string>();
            ValidatorsByFieldName = new Dictionary<string, IList<IMailChimpValidator>>();
        }
        
        public void MapField(XmlNode variableNode)
        {
            if (string.IsNullOrEmpty(variableNode.LocalName))
            {
                Global.Logger.Error("MapField received a bad node - missing localname [{0}]".FormatWith(variableNode.InnerXml));
                return;
            }
            if (!ValidatorsByFieldName.ContainsKey(variableNode.LocalName))
            {
                ValidatorsByFieldName.Add(variableNode.LocalName, new List<IMailChimpValidator>());
            }
            var validators = variableNode.GetAttribute("validator");
            if (string.IsNullOrEmpty(validators))
            {
                ValidatorsByFieldName[variableNode.LocalName].Add(MailChimpService.GetValidatorByKey("*"));
            }
            else
            {
                foreach (var validatorKey in validators.Split('|'))
                {
                    if (validatorKey.IsMatch(@"\w="))
                    {
                        var values = validatorKey.ExtractPatterns(@"(\w)=(.*)") as string[];
                        if (values == null || values.Length != 2)
                        {
                            Global.Logger.Warn("Validation incorrectly defined on node = {0} [{1}]".FormatWith(values.Dump(), variableNode.BaseURI));
                            continue;
                        }
                        MailChimpService.AddValidatorByKey(values.FirstOrDefault(), new Regex(values.LastOrDefault()));
                    }
                    var validator = MailChimpService.GetValidatorByKey(validatorKey);
                    if (validator == null)
                    {
                        Global.Logger.Warn("Validator not found with key supplied [{0}]".FormatWith(validatorKey));
                        continue;
                    }
                    ValidatorsByFieldName[variableNode.LocalName].Add(validator);
                }   
            }
            Fields.Add(variableNode.LocalName, variableNode.InnerText);
        }
    }
}