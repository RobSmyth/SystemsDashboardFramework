using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public interface IApplicationCommands
    {
        void BindView(TileConfiguration tile, FrameworkElement view, ITileLayoutController layoutController);
        void BindViewToAllCommands(UIElement element);
        CommandBinding SaveCommand { get; }
        CommandBinding CloseCommand { get; }
        CommandBinding DeleteCommand { get; }
        CommandBinding OpenCommand { get; }
    }
}