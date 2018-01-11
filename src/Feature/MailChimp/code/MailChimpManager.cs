using System;
using System.Collections.Generic;
using System.Linq;
using Lotus.Feature.MailChimp.Lists;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.Primitives;
using MailChimp.Helper;
using MailChimp.Lists;

namespace Lotus.Feature.MailChimp
{
    public class MailChimpManager : IMailChimpManager
    {
        public global::MailChimp.MailChimpManager Manager { get; set; }
        public IMailChimpList List { get; set; }
        public IMailChimpMergeVar MergeVar { get; set; }
        public string EmailType { get; set; }
        public bool DoubleOptin { get; set; }
        public bool UpdateExisting { get; set; }
        public bool ReplaceInterests { get; set; }
        public bool SendWelcome { get; set; }

        public MailChimpManager(global::MailChimp.MailChimpManager manager, IMailChimpList list)
        {
            Manager = manager;
            List = list;
            MergeVar = list.MergeVar;
            EmailType = "html";
            DoubleOptin = false;
            UpdateExisting = true;
            ReplaceInterests = true;
        }

        public bool Subscribe(IMailChimpSubscriber subscriber, string emailField = "email", bool skipValidate = false)
        {
            try
            {
                if (!skipValidate)
                {
                    var errors = subscriber.Validate(MergeVar).ToArray();
                    if (errors.Length > 0)
                    {
                        Global.Logger.Warn("MailChimpManager failed to validate subscriber = {0} [{1} dump({2})]".FormatWith(List.ListId, List.APIKey, subscriber.Fields.Dump()));
                        foreach (var error in errors)
                        {
                            Global.Logger.Warn("Error: {0}".FormatWith(error));
                        }
                        return false;   
                    }
                }
                var email = subscriber.GetAndCast<string>(emailField);
                if (string.IsNullOrEmpty(email))
                {
                    Global.Logger.Warn("MailChimpManager failed to generate manager - email undefined = {0} [{1} dump({2})]".FormatWith(List.ListId, List.APIKey, subscriber.Fields.Dump()));
                    return false;
                }
                var emailParameter = new EmailParameter()
                {
                    Email = email.ToString()
                };
                var mergeVar = new MergeVar();
                foreach (var mergeVarField in MergeVar.Fields)
                {
                    var targetField = mergeVarField.Key;
                    var targetValue = subscriber.Get(targetField);
                    if (targetValue != null)
                    {
                        mergeVar.Add(mergeVarField.Value, targetValue);
                    }
                }
                Manager.Subscribe(List.ListId, emailParameter, mergeVar, EmailType, DoubleOptin, UpdateExisting, ReplaceInterests, SendWelcome);
                return true;
            }
            catch (Exception exception)
            {
                Global.Logger.Error("Error sending subscriber to mailchimp = dump({0}) [apiKey = {1}]".FormatWith(MergeVar != null ? MergeVar.Fields.Dump() : string.Empty, List != null ? List.APIKey : "null"), exception);
                return false;
            }
        }
    }
}