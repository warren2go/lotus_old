using System;
using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Utils;
using Sitecore;
using Sitecore.Collections;
using StringUtil = Sitecore.StringUtil;

namespace Lotus.Foundation.Kernel.Services
{
    public class Tokenizer : IDisposable
    {
        public static readonly string TokenFormat = Settings.Tokenization.TokenFormat;
        public static readonly string TokenElementFormat = Settings.Tokenization.TokenElementFormat;
        public static readonly string TokenContextKey = Settings.Tokenization.TokenContextKey;
        public static readonly string TokenCharacters = Settings.Tokenization.TokenCharacters;
        public static readonly string TokenElementCharacters = Settings.Tokenization.TokenElementCharacters;
        public static readonly string TokenSelectRegex = @"(" + TokenFormat.Escape("{", "}").FormatWith(TokenCharacters + @"+?") + @")";
        public static readonly string TokenElementSelectRegex = @"(?:" + TokenFormat.Escape("{", "}").FormatWith(TokenCharacters + @"+?") + @")(" + TokenElementCharacters + @"+)";
        public static readonly string TokenAndElementSelectRegex = @"(" + TokenFormat.Escape("{", "}").FormatWith(TokenCharacters + @"+?") + TokenElementCharacters + @"+)";
        public static readonly string TokenElementParametersRegex = @"(\(.*\))";
        
        private SafeDictionary<string, object> _tokens = new SafeDictionary<string, object>();
        
        public bool Disposed;

        public void Dispose()
        {
            Disposed = true;

            _tokens.Clear();
        }

        /// <summary>
        /// Add a token to the tokenizer
        /// </summary>
        /// <param name="key">Token identifier to seek - without format (eg 'property' and not '$(property)'.</param>
        /// <param name="token">Instance of the token to use.</param>
        public void Add(string key, [NotNull] object token)
        {
            if (key.IsMatch(TokenSelectRegex))
                key = key.ExtractPattern(TokenSelectRegex);
            _tokens.Add(key, token);
        }

        /// <summary>
        /// Seek any methods to invoke for results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="allowedInvokes">(optional) The collection of allowed method-calls. default is null and will alllow all.</param>
        /// <returns>A resulting string with all methods invoked and tokens replaced by their corresponding results.</returns>
        public string Invoke(string format, [CanBeNull] string [] allowedInvokes = null)
        {
            return Invoke(format, _tokens, allowedInvokes);
        }

        /// <summary>
        /// Seek any methods to invoke for results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="tokens">Tokens to seek.</param>
        /// <param name="allowedInvokes">(optional) The collection of allowed method-calls. default is null and will alllow all.</param>
        /// <returns>A resulting string with all methods invoked and tokens replaced by their corresponding results.</returns>
        public static string Invoke(string format, SafeDictionary<string, object> tokens, [CanBeNull] string[] allowedInvokes = null)
        {
            return ExtractTokensAndElements(format).Aggregate(format, (current, tokenAndElement) =>
            {
                var value = tokens[ExtractTokenName(tokenAndElement)];
                
                var elements = ExtractTokenElements(tokenAndElement);

                foreach (var element in elements)
                {
                    if (allowedInvokes != null && StringUtil.Contains(ExtractTokenElementName(element), allowedInvokes))
                    {
                        //todo: introduce a Sitecore.DebugContext that can be used to fetch details as to whether debugging is on for modules (eg Sitecore.DebugContext.IsEnabled("Tokenizer"))
                        return current;   
                    }
                
                    if (!string.IsNullOrEmpty(element))
                    {
                        value = ReflectionUtil.GetResultWithPath(value, ExtractTokenElementName(element, false));
                    }   
                }

                return current.Replace(tokenAndElement, (value ?? tokenAndElement).ToString());
            });
        }
        
        /// <summary>
        /// Seek any tokens to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <returns>A resulting string with all tokens replaced by their corresponding variables.</returns>
        public string Replace(string format)
        {
            return Replace(format, _tokens);
        }

        /// <summary>
        /// Seek any tokens to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="tokens">Tokens to seek.</param>
        /// <returns>A resulting string with all tokens replaced by their corresponding variables.</returns>
        public static string Replace(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            foreach (var tokenAndElement in ExtractTokensAndElements(format))
            {
                var element = ExtractTokenElement(tokenAndElement);
                
                if (string.IsNullOrEmpty(element))
                {
                    format = ReplaceToken(format, tokens);
                }
                else
                {
                    format = ReplaceTokenElement(format, tokens, (current, key, value) => current.Replace(tokenAndElement, (ReflectionUtil.GetResultWithPath(value, element) ?? tokenAndElement).ToString()));
                }
            }
            return format;
        }

        /// <summary>
        /// Seek any tokens to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <returns>A resulting string with all tokens replaced by their corresponding variables.</returns>
        public string ReplaceToken(string format)
        {
            return ReplaceToken(format, _tokens);
        }

        /// <summary>
        /// Seek any tokens to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="tokens">Tokens to seek.</param>
        /// <returns>A resulting string with all tokens replaced by their corresponding variables.</returns>
        public static string ReplaceToken(string format, [NotNull] SafeDictionary<string, object> tokens)
        {
            return tokens.Aggregate(format, (current, token) => current.Replace(TokenFormat.FormatWith(token.Key), (token.Value ?? token.Key).ToString()));
        }

        /// <summary>
        /// Seek any tokens elements to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="func">A func to replace elements with - customize the result of the token element.</param>
        /// <returns>A resulting string with all token elements replaced by their corresponding variables.</returns>
        public string ReplaceTokenElement(string format, [NotNull] Func<string, string, object, string> func)
        {
            return ReplaceTokenElement(format, _tokens, func);
        }

        /// <summary>
        /// Seek any tokens elements to replace with variable results.
        /// </summary>
        /// <param name="format">Content that contains the tokens.</param>
        /// <param name="tokens">Tokens to seek.</param>
        /// <param name="func">A func to replace elements with - customize the result of the token element.</param>
        /// <returns>A resulting string with all token elements replaced by their corresponding variables.</returns>
        public static string ReplaceTokenElement(string format, [NotNull] SafeDictionary<string, object> tokens, [NotNull] Func<string, string, object, string> func)
        {
            return tokens.Aggregate(format, (current, token) => func.Invoke(current, token.Key, token.Value));
        }
        
        /// <summary>
        /// Extract a token with a specified index - 0 is default
        /// </summary>
        /// <param name="string">Content that contains the tokens.</param>
        /// <param name="index">Zero-based index for token to retrieve - 0 is default</param>
        /// <returns>The specified token or string.Empty.</returns>
        public static string ExtractToken(string @string, int index = 0)
        {
            var tokens = ExtractTokens(@string).ToArray();
            if (index > 0 && tokens.Length > index)
            {
                return tokens[index];
            }
            return tokens.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Extract all tokens found
        /// </summary>
        /// <param name="string">Content that contains the tokens.</param>
        /// <returns>A collection of tokens.</returns>
        public static IEnumerable<string> ExtractTokens(string @string)
        {
            return @string.ExtractPatterns(TokenSelectRegex);
        }

        /// <summary>
        /// Extract a token element with a specified index - 0 is default
        /// </summary>
        /// <param name="string">Content that contains the tokens.</param>
        /// <param name="index">Zero-based index for token element to retrieve - 0 is default</param>
        /// <returns>The specified token element or string.Empty.</returns>
        public static string ExtractTokenElement(string @string, int index = 0)
        {
            var tokenElements = ExtractTokenElements(@string).ToArray();
            if (index > 0 && tokenElements.Length > index)
            {
                return tokenElements[index];
            }
            return tokenElements.FirstOrDefault() ?? string.Empty;
        }
        
        /// <summary>
        /// Extract all token elements found
        /// </summary>
        /// <param name="string">Content that contains the token elements.</param>
        /// <returns>A collection of token elements.</returns>
        public static IEnumerable<string> ExtractTokenElements(string @string)
        {
            return @string.ExtractPatterns(TokenElementSelectRegex);
        }

        /// <summary>
        /// Extract a token with its element with a specified index - 0 is default
        /// </summary>
        /// <param name="string">Content that contains the tokens and their elements.</param>
        /// <param name="index">Zero-based index for token element to retrieve - 0 is default</param>
        /// <returns>The specified token and element or string.Empty.</returns>
        public static string ExtractTokenAndElement(string @string, int index = 0)
        {
            var tokenAndElements = ExtractTokensAndElements(@string).ToArray();
            if (index > 0 && tokenAndElements.Length > index)
            {
                return tokenAndElements[index];
            }
            return tokenAndElements.FirstOrDefault() ?? string.Empty;
        }
        
        /// <summary>
        /// Extract all tokens and their elements found
        /// </summary>
        /// <param name="string">Content that contains the tokens and their elements.</param>
        /// <returns>A collection of tokens and their elements.</returns>
        public static IEnumerable<string> ExtractTokensAndElements(string @string)
        {
            return @string.ExtractPatterns(TokenAndElementSelectRegex);
        }
        
        /// <summary>
        /// Extract the token identifier from the token
        /// </summary>
        /// <param name="token">Token to extract from.</param>
        /// <returns>Token identifier or string.Empty.</returns>
        public static string ExtractTokenName(string token)
        {
            return token.ExtractPattern(TokenFormat.Escape("{", "}").FormatWith(@"(" + TokenCharacters + @"+)"));
        }
        
        /// <summary>
        /// Extract the token element name from the token element
        /// </summary>
        /// <param name="token">Token element to extract name from.</param>
        /// <returns>Token element name or string.Empty.</returns>
        public static string ExtractTokenElementName(string token, bool justName = true)
        {
            if (justName)
                token = token.ReplacePattern(TokenElementParametersRegex, string.Empty);
            return token.ExtractPattern(TokenElementFormat.Escape("{", "}").FormatWith(@"(" + TokenCharacters + @"+)"));
        }

        public static string ExtractTokenElementParamaterString(string token, bool justparameters = false)
        {
            if (justparameters)
            {
                var parameters = StringUtil.RemovePrefix(token.ExtractPattern(TokenAndElementSelectRegex), "("); 
                return StringUtil.RemovePostfix(parameters, ")");
            }
            return token.ExtractPattern(TokenElementParametersRegex);
        }
    }
}