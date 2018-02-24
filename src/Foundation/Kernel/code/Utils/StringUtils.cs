using System.Collections;
using System.Collections.Generic;
using Lotus.Foundation.Kernel.Structures;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Web.UI.XamlSharp.Xaml.Extensions;

namespace Lotus.Foundation.Kernel.Utils
{
    public static class StringUtils
    {
        public static Tokenizer CreateTokenizer(params object[] tokens)
        {
            var tokenizer = new Tokenizer();
            foreach (var token in tokens)
                tokenizer.Add(nameof(token), token);
            return tokenizer;
        }

        public static Tokenizer CreateTokenizer(Dictionary<string, object> tokens)
        {
            var tokenizer = new Tokenizer();
            foreach (var token in tokens)
                tokenizer.Add(token.Key, token.Value);
            return tokenizer;
        }
        
        public static Tokenizer CreateTokenizer(SafeDictionary<string, object> tokens)
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
            return tokenizer.ResolveToken(format);
        }

        public static string CreateAndResolveTokenizer([NotNull] string format, params object[] tokens)
        {
            var tokenizer = CreateTokenizer(tokens);
            return ResolveTokenizer(tokenizer, format);
        }
        
        public static string CreateAndResolveTokenizer([NotNull] string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            var tokenizer = CreateTokenizer(tokens);
            return ResolveTokenizer(tokenizer, format);
        }

        public static string ExtractToken([NotNull] string @string, int index = 0)
        {
            return Tokenizer.ExtractToken(@string, index);
        }

        public static IEnumerable<string> ExtractTokens([NotNull] string @string)
        {
            return Tokenizer.ExtractTokens(@string);
        }
    }
}