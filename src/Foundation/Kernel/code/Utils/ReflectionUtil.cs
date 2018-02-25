using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sitecore;
using Sitecore.Reflection;

namespace Lotus.Foundation.Kernel.Utils
{
    public class ReflectionUtil
    {
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
            return name.Split('.').Where(x => !string.IsNullOrEmpty(x) && x.EndsWith("()")).ToArray();
        }

        private static string GetMethodName(IEnumerable<string> names)
        {
            return names.FirstOrDefault() ?? string.Empty;
        }

        private static string GetMethodName(string name)
        {
            return GetMethodNames(name).FirstOrDefault() ?? string.Empty;
        }
        
        public static object GetValue([NotNull] object target, string name)
        {
            return Sitecore.Reflection.ReflectionUtil.GetProperty(target, GetName(name)) ?? Sitecore.Reflection.ReflectionUtil.GetField(target, GetName(name));
        }

        public static object GetValueWithPath([NotNull] object target, string path)
        {
            return GetNames(path).Aggregate(target, GetValue);
        }

        public static T GetValueAndCast<T>([NotNull] object target, string name, T @default = default(T)) where T : class
        {
            return GetValue(target, name) as T ?? @default;
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