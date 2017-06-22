using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PCLExt.Config
{

    internal static class YamlExtensions
    {
#if DESKTOP || ANDROID || __IOS__ || MAC
        public static bool HasDefaultConstructor(this Type type) => type.GetTypeInfo().IsValueType || type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) != null;

        public static Type GetImplementedGenericInterface(Type type, Type genericInterfaceType) => GetImplementedInterfaces(type).FirstOrDefault(interfacetype => interfacetype.IsGenericType && interfacetype.GetGenericTypeDefinition() == genericInterfaceType);
        private static IEnumerable<Type> GetImplementedInterfaces(Type type)
        {
            if (type.IsInterface)
                yield return type;

            foreach (var implementedInterface in type.GetInterfaces())
                yield return implementedInterface;
        }

        public static PropertyInfo GetPublicProperty(this Type type, string name) => type.GetProperty(name);

        public static MethodInfo GetPublicInstanceMethod(this Type type, string name) => type.GetMethod(name, BindingFlags.Public | BindingFlags.Instance);
#endif
    }
}
