using System.ComponentModel;


namespace NoeticTools.TeamStatusBoard.Framework.DataSources
{
    public interface IDataPropertyViewModel : INotifyPropertyChanged
    { }

    public interface IDataPropertyViewModel<T> : IDataPropertyViewModel
    {
        T Value { get; }
    }
}