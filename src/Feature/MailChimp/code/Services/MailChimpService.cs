using Lotus.Feature.MailChimp.Lists;
using Lotus.Feature.MailChimp.Validators;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Logging;
using MailChimp;
using Sitecore.Collections;

namespace Lotus.Feature.MailChimp.Services
{
    public static class MailChimpService
    {
        internal static SafeDictionary<string, IMailChimpList> ListRepository = new SafeDictionary<string, IMailChimpList>();
        internal static SafeDictionary<string, IMailChimpValidator> ValidatorRepository = new SafeDictionary<string, IMailChimpValidator>();
        
        public static IMailChimpList GetListByID(string id)
        {
            var list = default(IMailChimpList);
            if (!ListRepository.TryGetValue(id, out list))
            {
                LLog.Warn("MailChimpList not found with ID supplied [{0}]".FormatWith(id));
            }
            return list;
        }

        public static void AddListByID(IMailChimpList list)
        {
            ListRepository.Add(list.ListId, list);
        }

        public static void CheckValidators()
        {
            var validator = default(IMailChimpValidator);
            if (!ValidatorRepository.TryGetValue("*", out validator))
            {
                ValidatorRepository.Add("*", new Regex(".*"));
            }
        }

        public static void AddValidatorByKey(string key, IMailChimpValidator validator)
        {
            ValidatorRepository.Add(key, validator);
        }
        
        public static IMailChimpValidator GetValidatorByKey(string key)
        {
            var validator = default(IMailChimpValidator);
            if (!ValidatorRepository.TryGetValue(key, out validator))
            {
                LLog.Warn("MailChimpValidator not found with Key supplied [{0}]".FormatWith(key));
            }
            return validator;
        }

        public static IMailChimpManager CreateManager(string listId)
        {
            var list = GetListByID(listId);
            if (list == null)
            {
                LLog.Warn("MailChimpManager could not be generated using the listId supplied [{0}]".FormatWith(listId));
                return null;
            }
            return new MailChimpManager(new global::MailChimp.MailChimpManager(list.Key), list);
        }
    }
}