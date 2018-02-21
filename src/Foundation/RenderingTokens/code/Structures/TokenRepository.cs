using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lotus.Foundation.Kernel.Structures.Collections;
using Sitecore.Collections;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Structures
{
    public class TokenRepository : IDisposable
    {
        public StaticDictionary<int, object> TokensByHashCode = new StaticDictionary<int, object>();

        public bool Disposed;

        public void Dispose()
        {
            Disposed = true;

            if (TokensByHashCode != null)
            {
                TokensByHashCode.ClearWith((pair, dispose) =>
                {
                    if (dispose)
                    {
                        var disposable = pair.Value as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                });   
            }
        }

        public static explicit operator SafeDictionary<string, string>(TokenRepository tokenRepository)
        {
            if (Settings.IsDebug)
                Global.Logger.Debug("Dumping repository: {0} Models".FormatWith(tokenRepository.TokensByHashCode.Count));
            
            var safeDictionary = new SafeDictionary<string, string>();
            foreach (var model in tokenRepository.TokensByHashCode.Values)
            {
                var modelType = model.GetType();
                var modelProperties = modelType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

                foreach (var modelProperty in modelProperties)
                {
                    if (Settings.IsDebug)
                        Global.Logger.Debug("Property: {0}".FormatWith(Settings.ResolveFormat.FormatWith(modelType.Name, modelProperty.Name)));
                    safeDictionary.Add(modelType.Name + "." + modelProperty.Name, modelProperty.GetValue(model).ToString());   
                }
            }
            return safeDictionary;
        }
        
        public void Add(int key, object value)
        {
            TokensByHashCode.Add(key, value);
        }

        public object GetFirstByType<T>()
        {
            return TokensByHashCode.Values.FirstOrDefault(ResolveType<T>);
        }

        private static bool ResolveType<T>(object value)
        {
            return value != null && value.GetType() == typeof(T);
        }
    }
}