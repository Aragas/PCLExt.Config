using System;

namespace PCLExt.Config
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConfigNameAttribute : Attribute
    {
        public string Name { get; }

        public ConfigNameAttribute(string name) { Name = name; }
    }
}