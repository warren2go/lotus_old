using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.RenderingTokens.Structures;
using Sitecore.Mvc.Common;
using Sitecore.Pipelines.RenderField;
using Sitecore.StringExtensions;
using StringExtensions = Lotus.Foundation.Extensions.Primitives.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Pipelines.RenderField
{
    public class ReplaceTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.Enabled)
                return;

            var tokenExtractPattern = args.RenderParameters[Settings.ParameterKeys.ExtractPattern] ?? args.Parameters[Settings.ParameterKeys.ExtractPattern];

            if (string.IsNullOrEmpty(tokenExtractPattern) && Settings.ForceReplace)
            {
                tokenExtractPattern = Settings.DefaultExtractPattern;
                
                if (Settings.IsDebug)
                    Global.Logger.Debug("No pattern supplied on rendering with key [{0}] but forcing replace = [{1}->{2}] using {3}".FormatWith(Settings.ParameterKeys.ExtractPattern, args.Item.ID, args.FieldName, tokenExtractPattern));
            }
            else if (string.IsNullOrEmpty(tokenExtractPattern))
            {
                if (Settings.IsDebug)
                    Global.Logger.Debug("No token pattern supplied on rendering with key [{0}] = [{1}->{2}]".FormatWith(Settings.ParameterKeys.ExtractPattern, args.Item.ID, args.FieldName));
                return;
            }

            var tokens = args.CustomData.Where(x => x.Key.IsMatch(StringExtensions.FormatWith(Settings.ResolveTokenFormat.FormatWith(x.Value)))).Select(x => x.Value).ToArray();
            
            var before = args.Result.ToString();
            
            args.Result.FirstPart = Replace(args.Result.FirstPart, tokens, tokenExtractPattern);
            args.Result.LastPart = Replace(args.Result.LastPart, tokens, tokenExtractPattern);
            args.WebEditParameters.Add("tokenPattern", tokenExtractPattern);

            var after = args.Result.ToString();
            
            if (Settings.IsDebug && before != after)
            {
                Global.Logger.Debug("Result: {0} -> {1}".FormatWith(before, after));
            }
        }

        private static string Replace(string value, object[] tokens, string tokenExtractPattern)
        {
            //todo: replace token here using reflection - find models in args.Parameters (safe collection of type/object)
            return value;
        }
    }
}