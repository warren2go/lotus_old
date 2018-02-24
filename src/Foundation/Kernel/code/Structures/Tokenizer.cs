using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lotus.Foundation.Kernel.Structures.Collections;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Reflection;

namespace Lotus.Foundation.Kernel.Structures
{
    public class Tokenizer : IDisposable
    {
        private const string _tokenCharacters = "[a-zA-Z0-9_]";
        private const string _tokenElementCharacters = "[a-zA-Z0-9._()]";
        
        public static readonly string TokenSelectRegex = @"(\$\(" + _tokenCharacters + @"+?\))";
        public static readonly string TokenElementSelectRegex = @"(?:\$\(" + _tokenCharacters + @"+?\))(" + _tokenElementCharacters + @"+)";
        public static readonly string TokenAndElementSelectRegex = @"(\$\(" + _tokenCharacters + @"+?\)" + _tokenElementCharacters + @"+)";
        public static readonly string TokenFormat = "$({0})";
        public static readonly string TokenElementFormat = ".{0}";

        public static readonly string TokenContextKey = "lotus-context-tokenizer";
        
        private SafeDictionary<string, object> _tokens = new SafeDictionary<string, object>();
        
        public bool Disposed;

        public void Dispose()
        {
            Disposed = true;

            _tokens.Clear();
        }

        public void Add(string key, object token)
        {
            if (key.IsMatch(TokenSelectRegex))
                key = key.ExtractPattern(TokenSelectRegex);
            _tokens.Add(key, token);
        }

        public static void Add(Tokenizer tokenizer, string key, object token)
        {
            tokenizer.Add(key, token);
        }

        public string Resolve(string format)
        {
            return Resolve(format, _tokens);
        }

        public static string Resolve(string format, SafeDictionary<string, object> tokens)
        {
            foreach (var tokenStr in ExtractTokensAndElements(format))
            {
                var tokenElementStr = ExtractTokenElement(tokenStr);
                
                if (string.IsNullOrEmpty(tokenElementStr))
                {
                    format = ResolveToken(format, tokens);
                }
                else
                {
                    format = ResolveTokenElement(format, tokens, (current, key, value) =>
                    {
                        var tokenObject = value;
                        var tokenElements = tokenElementStr.Split('.').Where(x => !string.IsNullOrEmpty(x));
                        foreach (var tokenElement in tokenElements)
                            tokenObject = ReflectionUtil.GetProperty(tokenObject, tokenElement) ?? ReflectionUtil.GetField(tokenObject, tokenElement);
                        return current.Replace(tokenStr, (tokenObject ?? tokenStr).ToString());
                    });
                }
            }
            return format;
        }

        public string ResolveToken(string format)
        {
            return ResolveToken(format, _tokens);
        }

        public static string ResolveToken(string format, SafeDictionary<string, object> tokens)
        {
            return tokens.Aggregate(format, (current, token) => current.Replace(TokenFormat.FormatWith(token.Key), token.Value.ToString()));
        }

        public string ResolveTokenElement(string format, Func<string, string, object, string> func)
        {
            return ResolveTokenElement(format, _tokens, func);
        }

        public static string ResolveTokenElement(string format, SafeDictionary<string, object> tokens, Func<string, string, object, string> func)
        {
            return tokens.Aggregate(format, (current, token) => func.Invoke(current, token.Key, token.Value));
        }
        
        public static string ExtractToken(string @string, int index = 0)
        {
            var tokens = ExtractTokens(@string).ToArray();
            if (index > 0 && tokens.Length > index)
            {
                return tokens[index];
            }
            return tokens.FirstOrDefault() ?? string.Empty;
        }

        public static IEnumerable<string> ExtractTokens(string @string)
        {
            return @string.ExtractPatterns(TokenSelectRegex);
        }

        public static string ExtractTokenElement(string @string, int index = 0)
        {
            var tokenElements = ExtractTokenElements(@string).ToArray();
            if (index > 0 && tokenElements.Length > index)
            {
                return tokenElements[index];
            }
            return tokenElements.FirstOrDefault() ?? string.Empty;
        }
        
        public static IEnumerable<string> ExtractTokenElements(string @string)
        {
            return @string.ExtractPatterns(TokenElementSelectRegex);
        }

        public static string ExtractTokenAndElement(string @string, int index = 0)
        {
            var tokenAndElements = ExtractTokensAndElements(@string).ToArray();
            if (index > 0 && tokenAndElements.Length > index)
            {
                return tokenAndElements[index];
            }
            return tokenAndElements.FirstOrDefault() ?? string.Empty;
        }
        
        public static IEnumerable<string> ExtractTokensAndElements(string @string)
        {
            return @string.ExtractPatterns(TokenAndElementSelectRegex);
        }
        
        public static string ExtractTokenName(string token)
        {
            return token.ExtractPattern(TokenFormat.Escape("{", "}").FormatWith(@"(" + _tokenCharacters + @"+)"));
        }
        
        public static string ExtractTokenElementName(string token)
        {
            return token.ExtractPattern(TokenElementFormat.Escape("{", "}").FormatWith(@"(" + _tokenCharacters + @"+)"));
        }
    }
}