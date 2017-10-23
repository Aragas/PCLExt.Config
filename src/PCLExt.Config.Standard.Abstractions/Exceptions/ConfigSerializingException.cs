using System;

namespace PCLExt.Config.Exceptions
{
    public class ConfigSerializingException : Exception
    {
        public ConfigSerializingException() { }
        public ConfigSerializingException(string message) : base(message) { }
        public ConfigSerializingException(string message, Exception innerException) : base(message, innerException) { }
    }
}