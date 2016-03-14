namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public interface IPropertiesDataService
    {
        IDataPropertyViewModel<T> Add<T>(string providerName, string propertyname, T value);
        bool Has<T>(string propertyname);
        IDataPropertyViewModel<T> Get<T>(string propertyName);
        IDataPropertyViewModel<T> Get<T>(string providerName, string propertyname);
    }
}