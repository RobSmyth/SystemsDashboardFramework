using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles
{
    internal interface ITileViewModel
    {
        ICommand ConfigureCommand { get; }
    }
}