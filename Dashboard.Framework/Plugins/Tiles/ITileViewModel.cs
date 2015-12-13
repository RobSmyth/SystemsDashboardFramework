using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles
{
    internal interface ITileViewModel
    {
        ICommand ConfigureCommand { get; }
    }
}