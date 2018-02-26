using System;
using System.Collections;
using System.Collections.Generic;
using Lotus.Foundation.Kernel.Services;
using Lotus.Foundation.Kernel.Structures;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Web.UI.XamlSharp.Xaml.Extensions;

namespace Lotus.Foundation.Kernel.Utils
{
    public static class TokenUtil
    {
        [NotNull]
        public static Tokenizer CreateTokenizer(params object[] tokens)
        {
            var tokenizer = new Tokenizer();
            foreach (var token in tokens)
                tokenizer.Add(nameof(token), token);
            return tokenizer;
        }

        [NotNull]
        public static Tokenizer CreateTokenizer([NotNull] Dictionary<string, object> tokens)
        {
            var tokenizer = new Tokenizer();
            foreach (var token in tokens)
                tokenizer.Add(token.Key, token.Value);
            return tokenizer;
        }
        
        [NotNull]
        public static Tokenizer CreateTokenizer([NotNull] SafeDictionary<string, object> tokens)
        {
            var tokenizer = new Tokenizer();
            foreach (var token in tokens)
                tokenizer.Add(token.Key, token.Value);
            return tokenizer;
        }

        public static string ResolveTokenizer([NotNull] Tokenizer tokenizer, [NotNull] string format)
        {
            if (tokenizer == null)
                return string.Empty;
            return tokenizer.ReplaceToken(format);
        }

        public static string CreateAndResolveTokenizer(string format, params object[] tokens)
        {
            var tokenizer = CreateTokenizer(tokens);
            return ResolveTokenizer(tokenizer, format);
        }
        
        public static string CreateAndResolveTokenizer(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            var tokenizer = CreateTokenizer(tokens);
            return ResolveTokenizer(tokenizer, format);
        }

        public static void Add([NotNull] Tokenizer tokenizer, string key, object token)
        {
            tokenizer.Add(key, token);
        }
        
        public static string Invoke([NotNull] Tokenizer tokenizer, string format)
        {
            return tokenizer.Invoke(format);
        }
        
        public static string Invoke(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            return Tokenizer.Invoke(format, tokens);
        }
        
        public static string Replace([NotNull] Tokenizer tokenizer, string format)
        {
            return tokenizer.Replace(format);
        }

        public static string Replace(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            return Tokenizer.Replace(format, tokens);
        }

        public static string ReplaceToken([NotNull] Tokenizer tokenizer, string format)
        {
            return tokenizer.ReplaceToken(format);
        }

        public static string ReplaceToken(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            return Tokenizer.ReplaceToken(format, tokens);
        }

        public static string ReplaceTokenElement([NotNull] Tokenizer tokenizer, string format, [NotNull] Func<string, string, object, string> func)
        {
            return tokenizer.ReplaceTokenElement(format, func);
        }

        public static string ReplaceTokenElement(string format, [NotNull] SafeDictionary<string, object> tokens, [NotNull] Func<string, string, object, string> func)
        {
            return Tokenizer.ReplaceTokenElement(format, tokens, func);
        }
    }
}