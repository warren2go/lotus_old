﻿using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Extensions.Casting;
using Lotus.Foundation.Extensions.Collections;
using Sitecore.StringExtensions;

namespace Lotus.Feature.MailChimp.Lists
{
    public class GenericSubscriber : IMailChimpSubscriber
    {
        public IDictionary<string, object> Fields { get; set; }

        public GenericSubscriber(params object[] parameters)
        {
            Fields = new Dictionary<string, object>();
            if (parameters.Length % 2 != 0)
            {
                Global.Logger.Warn("GenericSubscriber created with incorrect number of parameters = {0}".FormatWith(parameters.Dump()));
            }
            else
            {
                for (var i = 0; i < parameters.Length; i+=2)
                {
                    Fields.Add(parameters[i].ToString(), parameters[i+1]);
                }
            }
        }

        public void Add<T>(string key, T value)
        {
            Fields.Add(key, (object)value);
        }

        public object Get(string key)
        {
            var value = default(object);
            if (!Fields.TryGetValue(key, out value))
            {
                Global.Logger.Warn("GenericSubscriber does not contain a field with the key supplied = {0} [dump({1})]".FormatWith(key, Fields.Dump()));
            }
            return value;
        }

        public T GetAndCast<T>(string key)
        {
            var value = Get(key);
            return value != null ? value.CastTo<T>() : default(T);
        }
        
        public bool Validate(IMailChimpMergeVar mergeVar)
        {
            foreach (var validatorsForField in mergeVar.ValidatorsByFieldName)
            {
                var fieldName = validatorsForField.Key;
                var validators = validatorsForField.Value;
                var subscribeField = Get(fieldName);
                if (subscribeField == null && validators.Any(x => x.Key == "required"))
                {
                    Global.Logger.Error("GenericSubscriber is missing a field during validation = {0}".FormatWith(fieldName));
                    return false;
                }
                if (subscribeField != null)
                {
                    foreach (var validator in validators)
                    {
                        validator.Validate(subscribeField.ToString());
                    }
                }
            }
            return true;
        }
    }
}