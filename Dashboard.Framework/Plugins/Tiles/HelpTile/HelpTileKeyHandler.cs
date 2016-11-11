using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.HelpTile
{
    public sealed class HelpTileKeyHandler : IKeyHandler
    {
        private readonly IDashboardController _dashboardController;

        public HelpTileKeyHandler(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        bool IKeyHandler.CanHandle(Key key)
        {
            return Keyboard.Modifiers == ModifierKeys.None && key == Key.F1;
        }

        void IKeyHandler.Handle(Key key)
        {
            _dashboardController.ShowOnSidePane(new HelpViewController().CreateView(), "Help");
        }
    }
}