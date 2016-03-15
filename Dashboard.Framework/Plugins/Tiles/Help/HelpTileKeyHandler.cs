using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help
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