using System;

namespace PCLExt.Config.Exceptions
{
    public class ConfigDeserializingException : Exception
    {
        public ConfigDeserializingException() { }
        public ConfigDeserializingException(string message) : base(message) { }
        public ConfigDeserializingException(string message, Exception innerException) : base(message, innerException) { }
    }
}