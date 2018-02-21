using System.Collections.Generic;
using System.Linq;
using Lotus.Foundation.Extensions.RegularExpression;
using Lotus.Foundation.RenderingTokens.Structures;
using Sitecore.Mvc.Common;
using Sitecore.Pipelines.RenderField;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Pipelines.RenderField
{
    public class ReplaceTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.Enabled)
                return;

            var tokenPattern = args.RenderParameters["tokenPattern"] ?? args.Parameters["tokenPattern"];

            if (string.IsNullOrEmpty(tokenPattern) && Settings.ForceReplace)
            {
                tokenPattern = Settings.ExtractPattern;
                
                if (Settings.IsDebug)
                    Global.Logger.Debug("No pattern supplied on rendering but forcing replace = [{0}->{1}] using {2}".FormatWith(args.Item.ID, args.FieldName, tokenPattern));
            }
            else if (string.IsNullOrEmpty(tokenPattern))
            {
                if (Settings.IsDebug)
                    Global.Logger.Debug("No token pattern supplied on rendering = [{0}->{1}]".FormatWith(args.Item.ID, args.FieldName));
                return;
            }

            var tokenPairs = args.Parameters.Where(x => x.Key.IsMatch(Settings.ExtractPattern)).ToArray();
            
            var before = args.Result.ToString();
            
            args.Result.FirstPart = Replace(args.Result.FirstPart, tokenPairs, tokenPattern);
            args.Result.LastPart = Replace(args.Result.LastPart, tokenPairs, tokenPattern);
            args.WebEditParameters.Add("tokenPattern", tokenPattern);

            var after = args.Result.ToString();
            
            if (Settings.IsDebug && before != after)
            {
                Global.Logger.Debug("Result: {0} -> {1}".FormatWith(before, after));
            }
        }

        private static string Replace(string value, KeyValuePair<string, string>[] tokens, string tokenPattern)
        {
            //todo: replace token here using reflection - find models in args.Parameters (safe collection of type/object)
            return value;
        }
    }
}