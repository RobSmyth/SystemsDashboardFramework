namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public class NullDataPropertyViewModel<T> : IDataPropertyViewModel<T>
    {
        public T Value { get; }
    }
}