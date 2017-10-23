using PCLExt.FileStorage;

namespace PCLExt.Config
{
    public interface IConfigFile : IFile
    {
        ConfigType ConfigType { get; }
    }
}
