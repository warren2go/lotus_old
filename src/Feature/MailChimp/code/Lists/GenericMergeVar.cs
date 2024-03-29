﻿using System.Collections.Generic;
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
            var name = variableNode.GetAttribute("name");
            if (string.IsNullOrEmpty(name))
            {
                Global.Logger.Error("MapField received a bad node - missing name attribute [{0}]".FormatWith(variableNode.InnerXml));
                return;
            }
            if (!ValidatorsByFieldName.ContainsKey(name))
            {
                ValidatorsByFieldName.Add(name, new List<IMailChimpValidator>());
            }
            var validators = variableNode.GetAttribute("validation");
            if (string.IsNullOrEmpty(validators))
            {
                ValidatorsByFieldName[name].Add(MailChimpService.GetValidatorByKey("*"));
            }
            else
            {
                foreach (var validatorKey in validators.Split('|'))
                {
                    IMailChimpValidator validator;
                    if (validatorKey.IsMatch(@"\w+="))
                    {
                        var values = validatorKey.ExtractPatterns(@"(\w+)=(.*)").ToArray();
                        if (values.Length != 2)
                        {
                            Global.Logger.Warn("Validation incorrectly defined on node = {0} [{1}]".FormatWith(values.Dump(), variableNode.InnerXml));
                            continue;
                        }
                        Global.Logger.Info("Generic validator detected - {0}:{1} for {2}".FormatWith(values.FirstOrDefault(), values.LastOrDefault(), name));
                        validator = new Regex(values.LastOrDefault());
                    }
                    else
                    {
                        validator = MailChimpService.GetValidatorByKey(validatorKey);
                        if (validator == null)
                        {
                            Global.Logger.Warn("Validator not found with key supplied [{0}]".FormatWith(validatorKey));
                            continue;
                        }   
                    }
                    ValidatorsByFieldName[name].Add(validator);
                }   
            }
            Fields.Add(name, variableNode.InnerText);
        }
    }
}