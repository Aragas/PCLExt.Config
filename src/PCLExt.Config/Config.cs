using System;

namespace PCLExt.Config
{
    /// <summary>
    /// 
    /// </summary>
    public static class Config
    {
        private static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException(@"This functionality is not implemented in the portable version of this assembly.
You should reference the PCLExt.Config NuGet package from your main application project in order to reference the platform-specific implementation.");


        public static IConfig Create(ConfigType type)
        {
#if DESKTOP || ANDROID || __IOS__ || MAC
            switch (type)
            {
                case ConfigType.JsonConfig:
                    return new DesktopJsonConfig();

                case ConfigType.YamlConfig:
                    return new DesktopYamlConfig();
            }
#endif

            throw NotImplementedInReferenceAssembly();
        }
    }
}
