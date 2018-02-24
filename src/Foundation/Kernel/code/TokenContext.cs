using System;
using System.Web;
using Lotus.Foundation.Kernel.Structures;
using Sitecore.Collections;

namespace Sitecore
{
    public static class TokenContext
    {
        public static Tokenizer Tokenizer
        {
            get
            {
                if (Sitecore.Context.Items[Tokenizer.TokenContextKey] == null)
                    Sitecore.Context.Items[Tokenizer.TokenContextKey] = new Tokenizer();
                return Sitecore.Context.Items[Tokenizer.TokenContextKey] as Tokenizer;
            }
            set
            {
                Sitecore.Context.Items[Tokenizer.TokenContextKey] = value;
            }
        }

        public static void Add(string key, object token)
        {
            Tokenizer.Add(key, token);
        }

        public static string Resolve(string format)
        {
            return Tokenizer.Resolve(format);
        }

        public static string ResolveToken(string format)
        {
            return Tokenizer.ResolveToken(format);
        }

        public static string ResolveTokenElemet(string format, Func<string, string, object, string> func)
        {
            return Tokenizer.ResolveTokenElement(format, func);
        }
    }
}