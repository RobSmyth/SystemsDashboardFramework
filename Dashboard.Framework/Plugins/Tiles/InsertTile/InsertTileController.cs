using System.Collections.Generic;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTileController
    {
        private readonly ITileDragAndDropController _dragAndDropController;

        public InsertTileController(ITileProviderRegistry tileProviderRegistry, ITileDragAndDropController dragAndDropController)
        {
            _dragAndDropController = dragAndDropController;
            TileProviders = tileProviderRegistry.GetAll();
        }

        public IEnumerable<ITileControllerProvider> TileProviders { get; set; }

        public TileConfiguration Tile
        {
            get { return null; }
        }

        public FrameworkElement CreateView()
        {
            var view = new InsertTileControl {DataContext = this};
            _dragAndDropController.RegisterSource(view);
            return view;
        }
    }
}