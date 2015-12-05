using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Tiles.Help;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.Help
{
    public sealed class HelpTilePlugin : IPlugin, IKeyHandler
    {
        private readonly IDashboardController _dashboardController;

        public HelpTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

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

        public int Rank => 0;
    }
}