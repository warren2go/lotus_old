using System;
using System.Web;
using Lotus.Foundation.RenderingTokens.Structures;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Mvc.Helpers;

namespace Lotus.Foundation.RenderingTokens
{
    public static class Extensions
    {
        /// <summary>
        /// Render a field with support for replacing $(token)'s with valid models based on their declared names
        /// </summary>
        public static HtmlString FieldTokenized(this SitecoreHelper sitecoreHelper, string fieldName, Item item, params object[] tokenModels)
        {
            var tokenRepository = new TokenRepository();
            foreach (var model in tokenModels)
                tokenRepository.Add(model.GetHashCode(), model);
            return sitecoreHelper.Field(fieldName, item, (SafeDictionary<string, string>)tokenRepository);
        }
        
        /// <summary>
        /// Render a field with support for replacing $(token)'s with valid models based on their declared names
        /// </summary>
        public static HtmlString FieldTokenized(this SitecoreHelper sitecoreHelper, string fieldName, params object[] tokenModels)
        {
            var tokenRepository = new TokenRepository();
            foreach (var model in tokenModels)
                tokenRepository.Add(model.GetHashCode(), model);
            return sitecoreHelper.Field(fieldName, (SafeDictionary<string, string>)tokenRepository);
        }
        
        /// <summary>
        /// Render a field with support for replacing $(token)'s with valid models based on their declared names
        /// </summary>
        public static HtmlString FieldTokenized(this SitecoreHelper sitecoreHelper, string fieldName, TokenRepository tokenRepository)
        {
            return sitecoreHelper.Field(fieldName, (SafeDictionary<string, string>)tokenRepository);
        }
    }
}