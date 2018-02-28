using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Lotus.Foundation.Kernel.Extensions.Collections;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Lotus.Foundation.Kernel.Structures;
using Lotus.Foundation.Kernel.Utils;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Helpers;
using Sitecore.Pipelines.RenderField;
using Sitecore.Reflection;
using Sitecore.StringExtensions;
using ReflectionUtil = Sitecore.Reflection.ReflectionUtil;
using StringExtensions = Lotus.Foundation.Kernel.Extensions.Primitives.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Pipelines.RenderField
{
    public class InvokeTokens
    {
        public virtual void Process(RenderFieldArgs args)
        {
            if (!Settings.InvokeEnabled)
                return;
            
            if (Settings.ParameterCheck && string.IsNullOrEmpty(args.RenderParameters[Settings.ParameterKey] ?? args.Parameters[Settings.ParameterKey]))
                return;
            
            args.Result.FirstPart = Invoke(args.Result.FirstPart);
            args.Result.LastPart = Invoke(args.Result.LastPart);
        }

        private static string Invoke(string invoke)
        {
            return Sitecore.TokenContext.Invoke(invoke, Settings.InvokeWhitelist);
        }
    }
}