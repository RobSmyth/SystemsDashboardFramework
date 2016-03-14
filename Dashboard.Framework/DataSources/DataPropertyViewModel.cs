namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public class DataPropertyViewModel<T> : IDataPropertyViewModel<T>
    {
        public T Value { get; }
    }
}