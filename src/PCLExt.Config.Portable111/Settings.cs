using PCLExt.Config.Extensions;

namespace PCLExt.Config
{
    public abstract class ConfigProperties
    {
        protected abstract IConfigFile ConfigFile { get; }

        protected bool LoadConfig() => FileSystemExtensions.LoadConfig(ConfigFile, this);
        protected void SaveConfig() => FileSystemExtensions.SaveConfig(ConfigFile, this);
    }
}