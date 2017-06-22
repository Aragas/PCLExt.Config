using System;
using System.Reflection;

namespace PCLExt.Config.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasDefaultConstructor(this Type type) => type.GetConstructor(Type.EmptyTypes) != null;

    }
}
