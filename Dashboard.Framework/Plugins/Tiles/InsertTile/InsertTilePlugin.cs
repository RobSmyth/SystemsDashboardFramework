using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTilePlugin : IPlugin, IKeyHandler
    {
        private readonly IDashboardController _dashboardController;
        private readonly ApplicationServices _applicationServices;
        private readonly TileDragAndDropController _dragAndDropController;

        public InsertTilePlugin(IDashboardController dashboardController, ApplicationServices applicationServices, TileDragAndDropController dragAndDropController)
        {
            _dashboardController = dashboardController;
            _applicationServices = applicationServices;
            _dragAndDropController = dragAndDropController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(this);
        }

        bool IKeyHandler.CanHandle(Key key)
        {
            return Keyboard.Modifiers == ModifierKeys.None && key == Key.Insert;
        }

        void IKeyHandler.Handle(Key key)
        {
            _dashboardController.ShowOnSidePane(new InsertTileController(_applicationServices.TileProviders, _dragAndDropController).CreateView(), "Insert Tile");
        }
    }
}