using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lotus.Foundation.Kernel.Structures.Collections;
using Lotus.Foundation.RenderingTokens.Helpers;
using Sitecore.Collections;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.StringExtensions;

namespace Lotus.Foundation.RenderingTokens.Structures
{
    public class TokenRenderingArgs : PipelineArgs
    {
        public void Add(object model)
        {
            CustomData.Add(RenderingTokensHelper.ResolveTokenName(model), model);
        }

        public object GetFirstByType<T>()
        {
            return CustomData.Values.FirstOrDefault(ResolveType<T>);
        }

        private static bool ResolveType<T>(object value)
        {
            return value != null && value.GetType() == typeof(T);
        }
    }
}