using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.InsertTile
{
    public class InsertTilePlugin : IPlugin, IKeyHandler
    {
        private readonly IDashboardController _dashboardController;
        private readonly Services _services;
        private readonly TileDragAndDropController _dragAndDropController;

        public InsertTilePlugin(IDashboardController dashboardController, Services services, TileDragAndDropController dragAndDropController)
        {
            _dashboardController = dashboardController;
            _services = services;
            _dragAndDropController = dragAndDropController;
        }

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
            _dashboardController.ShowOnSidePane(new InsertTileController(_services.TileProviderRegistry, _dragAndDropController), "Insert Tile");
        }
    }
}