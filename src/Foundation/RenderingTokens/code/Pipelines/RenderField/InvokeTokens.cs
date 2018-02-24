using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Lotus.Foundation.Extensions.Collections;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Structures;
using Lotus.Foundation.Kernel.Utils;
using Lotus.Foundation.RenderingTokens.Helpers;
using Lotus.Foundation.RenderingTokens.Structures;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Helpers;
using Sitecore.Pipelines.RenderField;
using Sitecore.Reflection;
using Sitecore.StringExtensions;
using StringExtensions = Lotus.Foundation.Extensions.Primitives.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Pipelines.RenderField
{
    public class InvokeTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.Enabled)
                return;

            var tokenData = args.Data as SafeDictionary<string, object>;
            if (tokenData == null || tokenData.Count == 0)
                return;

            var tokens = tokenData.Where(x => x.Key.IsMatch(StringExtensions.FormatWith(Settings.ResolveTokenFormat.FormatWith(@".+")))).ToDictionary();

            var before = args.Result.ToString();
            
            args.Result.FirstPart = Invoke(args.Result.FirstPart, tokens);
            args.Result.LastPart = Invoke(args.Result.LastPart, tokens);

            var after = args.Result.ToString();
            
            if (Settings.IsDebug && before != after)
            {
                Global.Logger.Debug("Invoke: {0} -> {1}".FormatWith(before, after));
            }
        }

        private static string Invoke(string replace, Dictionary<string, object> tokens)
        {
            //todo: move this somewhere more elegant
            var tokenizer = StringUtils.CreateTokenizer(tokens);
            var tokenDefinitions = Tokenizer.ExtractTokensAndElements(replace);
            foreach (var tokenDefinition in tokenDefinitions)
            {
                var tokenIdentifier = Tokenizer.ExtractTokenName(tokenDefinition);
                var token = tokens.TryGetValueOrDefault(RenderingTokensHelper.ResolveTokenName(tokenIdentifier));
                if (token != null)
                {
                    var tokenElementDefinition = Tokenizer.ExtractTokenElement(tokenDefinition);
                    if (string.IsNullOrEmpty(tokenElementDefinition))
                    {
                        replace = tokenizer.ResolveToken(replace);
                    }
                    else
                    {
                        replace = tokenizer.ResolveTokenElement(replace, (current, key, value) =>
                        {
                            var tokenElements = tokenElementDefinition.Split('.').Where(x => !string.IsNullOrEmpty(x));
                            var resolvedValue = value;
                            foreach (var tokenElement in tokenElements)
                            {
                                var resolved = ReflectionUtil.GetProperty(resolvedValue, tokenElement) ?? ReflectionUtil.GetField(resolvedValue, tokenElement);
                                if (resolved != null)
                                {
                                    resolvedValue = resolved;
                                }
                            }
                            return current.Replace(tokenDefinition, resolvedValue.ToString());
                        });
                    }
                }
            }
            return replace;
        }
    }
}