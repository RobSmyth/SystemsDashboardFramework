using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Tiles.InsertTile
{
    public class InsertTileController : IViewController
    {
        public InsertTileController(ITileProviderRegistry tileProviderRegistry)
        {
            TileProviders = tileProviderRegistry.GetAll();
        }

        public IEnumerable<ITileControllerProvider> TileProviders { get; set; }

        public FrameworkElement CreateView()
        {
            return new InsertTileControl() { DataContext = this };
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}