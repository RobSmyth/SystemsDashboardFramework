namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public interface IDataPropertyViewModelFactory
    {
        IDataPropertyViewModel<T> Create<T>(string fullName);
    }
}