using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles
{
    public interface ITileViewModel
    {
        ICommand ConfigureCommand { get; }
    }
}