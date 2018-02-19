using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Kernel.Structures.Collections;
using Sitecore.StringExtensions;

namespace Lotus.Feature.MailChimp.Lists
{
    public class GenericSubscriber : IMailChimpSubscriber
    {
        public IDictionary<string, string> Fields { get; set; }

        public GenericSubscriber(NameValueCollection parameters)
        {
            Fields = new StaticDictionary<string, string>();
            foreach (var key in parameters.AllKeys)
            {
                Fields.Add(key, parameters.Get(key));
            }
        }

        public void Add(string key, string value)
        {
            Fields.Add(key, value);
        }

        public string Get(string key)
        {
            return Fields.TryGetValueOrDefault(key);
        }

        public T GetAndCast<T>(string key)
        {
            var value = Get(key);
            return value != null ? value.CastTo<T>() : default(T);
        }
        
        public IEnumerable<string> Validate(IMailChimpMergeVar mergeVar)
        {
            var errors = new List<string>();
            foreach (var validatorsForField in mergeVar.ValidatorsByFieldName)
            {
                var fieldName = validatorsForField.Key;
                var validators = validatorsForField.Value;
                var subscribeField = Get(fieldName);
                if (subscribeField == null && validators.Any(x => x.Key == "required"))
                {
                    Global.Logger.Error("GenericSubscriber is missing a field during validation = {0}".FormatWith(fieldName));
                    errors.Add("'{0}' is a required field but is empty or missing.".FormatWith(fieldName));
                }
                if (subscribeField != null)
                {
                    foreach (var validator in validators)
                    {
                        if (!validator.Validate(subscribeField.ToString()))
                        {
                            errors.Add("Validation for '{0}' failed using {1} with {2} validation".FormatWith(fieldName, subscribeField.ToString(), validator.Raw));
                        }
                    }
                }
            }
            return errors;
        }

        public override string ToString()
        {
            return Fields.Dump();
        }
    }
}