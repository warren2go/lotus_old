using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Lotus.Foundation.RenderingTokens.Helpers;
using Newtonsoft.Json;
using Sitecore.Collections;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Lotus.Foundation.RenderingTokens.Structures
{
    [Serializable]
    public class TokenRenderingArgs : EventArgs, ISerializable
    {
        private SafeDictionary<string, object> _tokens = new SafeDictionary<string, object>();
        
        public object Data
        {
            get
            {
                if (_tokens == null)
                {
                    _tokens = new SafeDictionary<string, object>();
                }
                return _tokens;
            }
            set { _tokens = value as SafeDictionary<string, object>; }
        }
        
        public void Add(object model)
        {
            _tokens.Add(RenderingTokensHelper.ResolveTokenName(model), model);
        }
        
        public void Add(string key, object model)
        {
            _tokens.Add(RenderingTokensHelper.ResolveTokenName(key), model);
        }

        protected string SerializeTokens(SafeDictionary<string, object> tokens)
        {
            object obj = (object) null;
            if (tokens != null)
                obj = (object) tokens.ToArray<KeyValuePair<string, object>>();
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            });
        }
        
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Assert.ArgumentNotNull((object) info, nameof (info));
            SerializeTokens(_tokens);
        }
    }
}