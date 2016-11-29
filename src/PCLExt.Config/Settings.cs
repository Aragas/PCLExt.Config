using PCLExt.Config.Extensions;

namespace PCLExt.Config
{
    public abstract class ConfigProperties
    {
        protected abstract string FileName { get; }
        protected abstract ConfigType ConfigType { get; }

        protected bool LoadConfig() => FileSystemExtensions.LoadConfig(ConfigType, FileName, this);
        protected void SaveConfig() => FileSystemExtensions.SaveConfig(ConfigType, FileName, this);
    }
}