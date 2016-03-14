namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public interface IDataPropertyViewModel
    { }

    public interface IDataPropertyViewModel<T> : IDataPropertyViewModel
    {
        T Value { get; }
    }
}