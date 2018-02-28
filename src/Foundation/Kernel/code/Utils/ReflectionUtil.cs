using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Sitecore;
using Sitecore.Reflection;

namespace Lotus.Foundation.Kernel.Utils
{
    public class ReflectionUtil
    {
        private const string _validNameRegex = @"[\.]?([a-zA-Z0-9_]+)[\(]?";
        private const string _validParametersRegex = @"(\(.*\))";
        
        private static string[] GetNames(string name)
        {
            return name.Split('.').Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        private static string GetName(IEnumerable<string> names)
        {
            return names.FirstOrDefault() ?? string.Empty;
        }

        private static string GetName(string name)
        {
            return GetNames(name).FirstOrDefault() ?? string.Empty;
        }
        
        private static string[] GetMethodNames(string name)
        {
            return name.Split('.').Where(x => !string.IsNullOrEmpty(x) && x.IsMatch(_validParametersRegex)).ToArray();
        }

        private static string GetMethodName(IEnumerable<string> names)
        {
            return names.FirstOrDefault() ?? string.Empty;
        }

        private static string GetMethodName(string name)
        {
            return GetMethodNames(name).FirstOrDefault() ?? string.Empty;
        }
        
        public static object GetResult([NotNull] object target, string name)
        {
            if (name.EndsWith(")"))
            {
                return InvokeMethod(target, name.ExtractPattern(_validNameRegex));
            }
            return Sitecore.Reflection.ReflectionUtil.GetProperty(target, name.ExtractPattern(_validNameRegex)) ?? Sitecore.Reflection.ReflectionUtil.GetField(target, name.ExtractPattern(_validNameRegex));
        }

        public static object GetResultWithPath([NotNull] object target, string path)
        {
            return GetNames(path).Aggregate(target, GetResult);
        }

        public static T GetResultAndCast<T>([NotNull] object target, string name, T @default = default(T)) where T : class
        {
            return GetResult(target, name) as T ?? @default;
        }

        public static MethodInfo GetMethod([NotNull] object target, string methodName, params object[] parameters)
        {
            return Sitecore.Reflection.ReflectionUtil.GetMethod(target, methodName, parameters);
        }
        
        public static object InvokeMethod([NotNull] object target, string methodName, params object[] parameters)
        {
            return Sitecore.Reflection.ReflectionUtil.InvokeMethod(GetMethod(target, methodName, parameters), parameters, target);
        }
        
        public static T InvokeMethodAndCast<T>([NotNull] object target, string methodName, params object[] parameters) where T : class
        {
            return Sitecore.Reflection.ReflectionUtil.InvokeMethod(GetMethod(target, methodName, parameters), parameters, target) as T;
        }
    }
}