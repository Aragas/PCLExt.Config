using System.IO;
using System.Threading.Tasks;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;

namespace PCLExt.Config.Extensions
{
    public static class FileSystemExtensions
    {
        public static bool LoadConfig<T>(IConfigFile file, T value) where T : class
        {
            var config = Config.Create(file.ConfigType);

            try
            {
                var content = file.ReadAllText();
                if (!string.IsNullOrEmpty(content))
                {
                    if (value == null)
                        value = config.Deserialize<T>(content);
                    else
                    {
                        config.PopulateObject(content, value);
                        file.WriteAllText(config.Serialize(value));
                    }
                }
                else
                    file.WriteAllText(config.Serialize(value));
            }
            catch (ConfigDeserializingException)
            {
                file.WriteAllText(config.Serialize(value));
                return false;
            }
            catch (ConfigSerializingException) { return false; }

            return true;
        }
        public static bool SaveConfig<T>(IConfigFile file, T defaultValue = default(T)) where T : class
        {
            var config = Config.Create(file.ConfigType);

            try { file.WriteAllText(config.Serialize(defaultValue)); }
            catch (ConfigSerializingException) { return false; }

            return true;
        }

        public static async Task<bool> LoadConfigAsync<T>(IConfigFile file, T value) where T : class
        {
            var config = Config.Create(file.ConfigType);

            try
            {
                var content = await file.ReadAllTextAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    if (value == null)
                        value = config.Deserialize<T>(content);
                    else
                    {
                        config.PopulateObject(content, value);
                        await file.WriteAllTextAsync(config.Serialize(value));
                    }
                }
                else
                    await file.WriteAllTextAsync(config.Serialize(value));
            }
            catch (ConfigDeserializingException)
            {
                await file.WriteAllTextAsync(config.Serialize(value));
                return false;
            }
            catch (ConfigSerializingException) { return false; }

            return true;
        }
        public static async Task<bool> SaveConfigAsync<T>(IConfigFile file, T defaultValue = default(T)) where T : class
        {
            var config = Config.Create(file.ConfigType);

            try { await file.WriteAllTextAsync(config.Serialize(defaultValue)); }
            catch (ConfigSerializingException) { return false; }

            return true;
        }

        public static bool LoadLog(IFile file, out string content)
        {
            content = string.Empty;

            try { content = file.ReadAllText(); }
            catch (IOException) { return false; }

            return true;
        }
        public static bool SaveLog(IFile file, string content)
        {
            try { file.WriteAllText(content); }
            catch (IOException) { return false; }

            return true;
        }
    }
}