using System;
using System.Globalization;
using Lotus.Foundation.Kernel.Extensions.Primitives;
using Sitecore.Diagnostics;
using Sitecore;

namespace Lotus.Foundation.Kernel.Extensions.Casting
{
    public static class CastingExtensions
    {
        public static bool TryCastTo<T>(this object instance, [CanBeNull] out T casted)
        {
            try
            {
                casted = instance.CastTo<T>();
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Error casting type - " + instance.GetType().FullName + " to " + typeof(T).FullName, exception);
                casted = default(T);
                return false;
            }
        }

        [CanBeNull]
        public static T CastTo<T>(this object instance)
        {
            T casted;

            try
            {
                // we need to specify exact conversions for strings
                if (instance is string)
                {
                    switch (typeof(T).Name.ToLower())
                    {
                        case "boolean":
                            instance = instance.ToString().ToBool();
                            break;

                        case "int16":
                            instance = Int16.Parse(instance.ToString());
                            break;

                        case "int32":
                            instance = Int32.Parse(instance.ToString());
                            break;

                        case "int64":
                            instance = Int64.Parse(instance.ToString());
                            break;

                        case "uint16":
                            instance = UInt16.Parse(instance.ToString());
                            break;

                        case "uint32":
                            instance = UInt32.Parse(instance.ToString());
                            break;

                        case "uint64":
                            instance = UInt64.Parse(instance.ToString());
                            break;

                        case "datetime":
                            instance = DateTime.Parse(instance.ToString());
                            break;
                    }
                }

                casted = (T)System.Convert.ChangeType(instance, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                Log.Error("Error casting type - " + instance.GetType().FullName + " to " + typeof(T).FullName, exception);
                #if DEBUG
                throw;
                #endif
            }

            return casted;
        }
    }
}