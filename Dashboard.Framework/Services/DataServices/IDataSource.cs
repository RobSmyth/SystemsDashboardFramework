namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataSource
    {
        string Name { get; }
        void Write<T>(string name, T value);
        T Read<T>(string name);
    }
}