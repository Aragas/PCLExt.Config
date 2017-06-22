namespace PCLExt.Config.Extensions
{
    public static class ConfigTypeExtensions
    {
        public static string GetFileExtension(this ConfigType configType)
        {
            switch (configType)
            {
                case ConfigType.JsonConfig:
                    return ".json";
                case ConfigType.YamlConfig:
                    return ".yml";
            }

            return ".none";
        }
    }
}
