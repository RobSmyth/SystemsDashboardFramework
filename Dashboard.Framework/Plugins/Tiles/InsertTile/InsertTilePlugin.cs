using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Input;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTilePlugin : IPlugin, IKeyHandler
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
            _dashboardController.ShowOnSidePane(new InsertTileController(_services.TileProviders, _dragAndDropController), "Insert Tile");
        }
    }
}