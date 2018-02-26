using System;
using System.Web;
using Lotus.Foundation.Kernel.Services;
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

        public static string Invoke(string format, string[] allowedInvokes = null)
        {
            return Tokenizer.Invoke(format, allowedInvokes);
        }

        public static string Invoke(string format, SafeDictionary<string, object> tokens, string[] allowedInvokes = null)
        {
            return Tokenizer.Invoke(format, tokens, allowedInvokes);
        }
        
        public static string Replace(string format)
        {
            return Tokenizer.Replace(format);
        }

        public static string Replace(string format, SafeDictionary<string, object> tokens)
        {
            return Tokenizer.Replace(format, tokens);
        }

        public static string ReplaceToken(string format)
        {
            return Tokenizer.ReplaceToken(format);
        }

        public static string ReplaceToken(string format, SafeDictionary<string, object> tokens)
        {
            return Tokenizer.ReplaceToken(format, tokens);
        }

        public static string ReplaceTokenElement(string format, Func<string, string, object, string> func)
        {
            return Tokenizer.ReplaceTokenElement(format, func);
        }

        public static string ReplaceTokenElement(string format, SafeDictionary<string, object> tokens, Func<string, string, object, string> func)
        {
            return Tokenizer.ReplaceTokenElement(format, tokens, func);
        }
    }
}