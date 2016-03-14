using System.ComponentModel;


namespace NoeticTools.SystemsDashboard.Framework.DataSources
{
    public interface IDataPropertyViewModel : INotifyPropertyChanged
    { }

    public interface IDataPropertyViewModel<T> : IDataPropertyViewModel
    {
        T Value { get; }
    }
}