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
    public class ReplaceTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.Enabled)
                return;

            var before = args.Result.ToString();
            
            args.Result.FirstPart = Replace(args.Result.FirstPart);
            args.Result.LastPart = Replace(args.Result.LastPart);

            var after = args.Result.ToString();
            
            if (Settings.IsDebug && before != after)
            {
                Global.Logger.Debug("Replace: {0} -> {1}".FormatWith(before, after));
            }
        }

        private static string Replace(string replace)
        {
            return Sitecore.TokenContext.Resolve(replace);
        }
    }
}