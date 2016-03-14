namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public sealed class DataPropertyViewModelFactory : IDataPropertyViewModelFactory
    {
        public IDataPropertyViewModel<T> Create<T>(string fullName)
        {
            return new DataPropertyViewModel<T>();
        }
    }
}