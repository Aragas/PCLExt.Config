using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using PCLExt.Config.Exceptions;

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.TypeInspectors;

namespace PCLExt.Config
{
    internal class DesktopYamlConfig : IConfig
    {
        public string Serialize<T>(T target)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var serializer = new SerializerBuilder().EmitDefaults().WithTypeInspector(inspector => new ConfigTypeInspector(inspector)).Build();
                    serializer.Serialize(stringWriter, target);
                    return stringWriter.ToString();
                }
            }
            catch (YamlException ex) { throw new ConfigSerializingException(string.Empty, ex); }
        }
        public T Deserialize<T>(string value)
        {
            try
            {
                var deserializer = new DeserializerBuilder().WithTypeInspector(inspector => new ConfigTypeInspector(inspector)).Build();
                return (T) deserializer.Deserialize(new StringReader(value), typeof(T));
            }
            catch (YamlException ex) { throw new ConfigDeserializingException(string.Empty, ex); }
        }
        public void PopulateObject<T>(string value, T target)
        {
            try
            {
                var deserializer = new DeserializerBuilder().WithObjectFactory(new LambdaObjectFactory(Factory)).Build();
                var source = (T) deserializer.Deserialize(new StringReader(value), target.GetType());
                CopyAll(source, target);
            }
            catch (YamlException ex) { throw new ConfigDeserializingException(string.Empty, ex); }
        }

        private static object Factory(Type type)
        {
            if (type == typeof(string))
                return string.Empty;
            else if (type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null) // HasDefaultConstructor
                return Activator.CreateInstance(type);
            else
                return FormatterServices.GetUninitializedObject(type);
        }

        private static void CopyAll<T>(T source, T target)
        {
            var type = target.GetType();
            foreach (var sourceProperty in type.GetRuntimeProperties().Where(prop => prop.CanRead && prop.GetMethod.IsPublic && prop.CustomAttributes.All(att => att.AttributeType != typeof(YamlIgnoreAttribute))))
            {
                var targetProperty = type.GetRuntimeProperty(sourceProperty.Name);
                if (targetProperty.CanWrite)
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
            foreach (var sourceField in type.GetRuntimeFields().Where(field => field.IsPublic && field.CustomAttributes.All(att => att.AttributeType != typeof(YamlIgnoreAttribute))))
            {
                var targetField = type.GetRuntimeField(sourceField.Name);
                targetField.SetValue(target, sourceField.GetValue(source));
            }
        }
    }

    internal class ConfigTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _innerTypeInspector;

        public ConfigTypeInspector(ITypeInspector typeInspector) { _innerTypeInspector = typeInspector; }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container) =>
            _innerTypeInspector.GetProperties(type, container)
                .Where(p => p.GetCustomAttribute<ConfigIgnoreAttribute>() == null)
                .Select(p =>
                {
                    var descriptor = new PropertyDescriptor(p);

                    var member = p.GetCustomAttribute<ConfigNameAttribute>();
                    if (member != null)
                    {
                        if (!string.IsNullOrEmpty(member.Name))
                            descriptor.Name = member.Name;

                    }

                    return (IPropertyDescriptor) descriptor;
                });
    }
}
