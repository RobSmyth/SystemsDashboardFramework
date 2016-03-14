namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public class NullDataPropertyViewModel<T> : NotifyingViewModelBase, IDataPropertyViewModel<T>
    {
        public T Value { get; }
    }
}