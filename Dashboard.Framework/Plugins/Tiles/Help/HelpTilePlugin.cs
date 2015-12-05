using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Tiles.Help;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help
{
    public sealed class HelpTilePlugin : IPlugin, IKeyHandler
    {
        private readonly IDashboardController _dashboardController;

        public HelpTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(this);
        }

        bool IKeyHandler.CanHandle(Key key)
        {
            return Keyboard.Modifiers == ModifierKeys.None && key == Key.F1;
        }

        void IKeyHandler.Handle(Key key)
        {
            _dashboardController.ShowOnSidePane(new HelpViewController(), "Help");
            ;
        }
    }
}