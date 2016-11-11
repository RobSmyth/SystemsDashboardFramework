using System.Windows;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public interface IApplicationCommands
    {
        CommandBinding SaveCommand { get; }
        CommandBinding CloseCommand { get; }
        CommandBinding DeleteCommand { get; }
        CommandBinding OpenCommand { get; }
        void BindView(TileConfiguration tile, FrameworkElement view, ITileLayoutController layoutController);
        void BindViewToAllCommands(UIElement element);
    }
}