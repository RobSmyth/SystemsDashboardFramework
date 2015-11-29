using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.InsertTile
{
    public class InsertTileController : IViewController
    {
        private readonly TileDragAndDropController _dragAndDropController;

        public InsertTileController(ITileProviderRegistry tileProviderRegistry, TileDragAndDropController dragAndDropController)
        {
            _dragAndDropController = dragAndDropController;
            TileProviders = tileProviderRegistry.GetAll();
        }

        public IEnumerable<ITileControllerProvider> TileProviders { get; set; }

        public FrameworkElement CreateView()
        {
            var view = new InsertTileControl() { DataContext = this };
            _dragAndDropController.RegisterSource(view);
            return view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}