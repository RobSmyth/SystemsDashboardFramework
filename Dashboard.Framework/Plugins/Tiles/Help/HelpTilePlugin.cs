using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Tiles.Help
{
    public class HelpTilePlugin : IPlugin, IKeyHandler
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
    }
}