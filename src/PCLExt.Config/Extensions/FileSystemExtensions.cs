using System.IO;

using PCLExt.FileStorage;

namespace PCLExt.Config.Extensions
{
    public static class FileSystemExtensions
    {
        public static bool LoadConfig<T>(ConfigType configType, string filename, T value, int millisecondsTimeout = 5000) where T : class
        {
            var config = Config.Create(configType);
            var file = Storage.SettingsFolder.CreateFileAsync($"{filename}.{config.FileExtension}", CreationCollisionOption.OpenIfExists).Result;

            try
            {
                var content = file.ReadAllTextAsync().Result;
                if (!string.IsNullOrEmpty(content))
                {
                    if (value == null)
                        value = config.Deserialize<T>(content);
                    else
                    {
                        config.PopulateObject(content, value);
                        file.WriteAllTextAsync(config.Serialize(value)).Wait(millisecondsTimeout);
                    }
                }
                else
                    file.WriteAllTextAsync(config.Serialize(value)).Wait(millisecondsTimeout);
            }
            catch (ConfigDeserializingException)
            {
                file.WriteAllTextAsync(config.Serialize(value)).Wait(millisecondsTimeout);
                return false;
            }
            catch (ConfigSerializingException) { return false; }

            return true;
        }
        public static bool SaveConfig<T>(ConfigType configType, string filename, T defaultValue = default(T), int millisecondsTimeout = 5000) where T : class
        {
            var config = Config.Create(configType);
            var file = Storage.SettingsFolder.CreateFileAsync($"{filename}.{config.FileExtension}", CreationCollisionOption.OpenIfExists).Result;

            try { file.WriteAllTextAsync(config.Serialize(defaultValue)).Wait(millisecondsTimeout); }
            catch (ConfigSerializingException) { return false; }

            return true;
        }

        public static bool LoadLog(string filename, out string content, int millisecondsTimeout = 5000)
        {
            content = string.Empty;
            var file = Storage.LogFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result;

            try { content = file.ReadAllTextAsync().Result; }
            catch (IOException) { return false; }

            return true;
        }
        public static bool SaveLog(string filename, string content, int millisecondsTimeout = 5000)
        {
            var file = Storage.LogFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result;

            try { file.WriteAllTextAsync(content).Wait(millisecondsTimeout); }
            catch (IOException) { return false; }

            return true;
        }
    }
}