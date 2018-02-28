using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
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
using StringExtensions = Lotus.Foundation.Kernel.Extensions.Primitives.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Pipelines.RenderField
{
    public class ReplaceTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.Enabled)
                return;

            args.Result.FirstPart = Replace(args.Result.FirstPart);
            args.Result.LastPart = Replace(args.Result.LastPart);
        }

        private static string Replace(string replace)
        {
            return Sitecore.TokenContext.Replace(replace);
        }
    }
}