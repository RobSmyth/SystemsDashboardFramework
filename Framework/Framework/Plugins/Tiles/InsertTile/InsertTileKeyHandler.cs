using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Input;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTileKeyHandler : IKeyHandler
    {
        private readonly IDashboardController _dashboardController;
        private readonly ITileDragAndDropController _dragAndDropController;
        private readonly IServices _services;

        public InsertTileKeyHandler(IDashboardController dashboardController, ITileDragAndDropController dragAndDropController, IServices services)
        {
            _dashboardController = dashboardController;
            _dragAndDropController = dragAndDropController;
            _services = services;
        }

        bool IKeyHandler.CanHandle(Key key)
        {
            return Keyboard.Modifiers == ModifierKeys.None && key == Key.Insert;
        }

        void IKeyHandler.Handle(Key key)
        {
            _dashboardController.ShowOnSidePane(new InsertTileController(_services.TileProviders, _dragAndDropController).CreateView(), "Insert Tile");
        }
    }
}