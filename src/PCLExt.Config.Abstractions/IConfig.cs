using System;

namespace PCLExt.Config
{
    public enum ConfigType { JsonConfig, YamlConfig }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ConfigIgnoreAttribute : Attribute { }

    public class ConfigException : Exception
    {
        public ConfigException() { }
        public ConfigException(string message) : base(message) { }
        public ConfigException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class ConfigSerializingException : Exception
    {
        public ConfigSerializingException() { }
        public ConfigSerializingException(string message) : base(message) { }
        public ConfigSerializingException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class ConfigDeserializingException : Exception
    {
        public ConfigDeserializingException() { }
        public ConfigDeserializingException(string message) : base(message) { }
        public ConfigDeserializingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public interface IConfig
    {
        string FileExtension { get; }

        string Serialize<T>(T target);
        T Deserialize<T>(string value);
        void PopulateObject<T>(string value, T target);

    }
}