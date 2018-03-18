using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lotus.Foundation.Kernel.Extensions.Casting;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Lotus.Foundation.Kernel.Extensions.RegularExpression;
using Sitecore;
using Sitecore.Reflection;

namespace Lotus.Foundation.Kernel.Utils
{
    public class ReflectionUtil
    {
        private const string _validNameRegex = @"[\.]?([a-zA-Z0-9_]+)[\(]?";
        private const string _validParametersRegex = @"(\(.*\))";
        
        [NotNull]
        private static string[] GetNames(string name)
        {
            return name.Split('.').Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        [NotNull]
        private static string GetName(IEnumerable<string> names)
        {
            return names.FirstOrDefault() ?? string.Empty;
        }

        [NotNull]
        private static string GetName(string name)
        {
            return GetNames(name).FirstOrDefault() ?? string.Empty;
        }
        
        [NotNull]
        private static string[] GetMethodNames(string name)
        {
            return name.Split('.').Where(x => !string.IsNullOrEmpty(x) && x.IsMatch(_validParametersRegex)).ToArray();
        }

        [NotNull]
        private static string GetMethodName(IEnumerable<string> names)
        {
            return names.FirstOrDefault() ?? string.Empty;
        }

        [NotNull]
        private static string GetMethodName(string name)
        {
            return GetMethodNames(name).FirstOrDefault() ?? string.Empty;
        }
        
        [CanBeNull]
        public static object GetResult([NotNull] object target, string name)
        {
            return Sitecore.Reflection.ReflectionUtil.GetProperty(target, name.ExtractPattern(_validNameRegex)) ?? Sitecore.Reflection.ReflectionUtil.GetField(target, name.ExtractPattern(_validNameRegex));
        }

        [CanBeNull]
        public static object GetResultWithPath([NotNull] object target, string path, params string[] allowedMethods)
        {
            return GetNames(path).Aggregate(target, delegate(object value, string name)
            {
                if (name.EndsWith(")"))
                {
                    if (name.ExtractPattern(_validNameRegex).ContainsRegex(allowedMethods))
                    {
                        return InvokeMethod(value, name.ExtractPattern(_validNameRegex));
                    }
                }
                return GetResult(value, name);
            });
        }

        [CanBeNull]
        public static T GetResultAndCast<T>([NotNull] object target, string name, T @default = default(T)) where T : class
        {
            return GetResult(target, name) as T ?? @default;
        }

        [CanBeNull]
        public static MethodInfo GetMethod([NotNull] object target, string methodName, params object[] parameters)
        {
            return Sitecore.Reflection.ReflectionUtil.GetMethod(target, methodName, parameters);
        }
        
        [CanBeNull]
        public static object InvokeMethod([NotNull] object target, string methodName, params object[] parameters)
        {
            return Sitecore.Reflection.ReflectionUtil.InvokeMethod(GetMethod(target, methodName, parameters), parameters, target);
        }
        
        [CanBeNull]
        public static T InvokeMethodAndCast<T>([NotNull] object target, string methodName, params object[] parameters) where T : class
        {
            return Sitecore.Reflection.ReflectionUtil.InvokeMethod(GetMethod(target, methodName, parameters), parameters, target) as T;
        }

        [CanBeNull]
        public static T CastTo<T>([CanBeNull] object value, [CanBeNull] T @default = default(T))
        {
            if (value == null)
                return @default;
            return value.CastTo<T>();
        }

        [CanBeNull]
        public static T TryCastTo<T>([CanBeNull] object value, [CanBeNull] T @default = default(T))
        {
            if (value == null)
                return @default;
            T casted;
            return value.TryCastTo(out casted) ? casted : @default;
        }
    }
}