using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Tiles.Dashboards
{
    public interface ICloseableViewModel
    {
        ICommand CloseCommand { get; }
    }
}