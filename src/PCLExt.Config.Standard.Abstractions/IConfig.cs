namespace PCLExt.Config
{
    public interface IConfig
    {
        string Serialize<T>(T target);
        T Deserialize<T>(string value);
        void PopulateObject<T>(string value, T target);
    }
}