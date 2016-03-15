using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    public sealed class BlankTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private const string TileTypeId = "Blank.Tile";

        public BlankTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Blank";

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new BlankTileControl();
            new BlankTileViewModel(tile, _dashboardController, layoutController, _services, view);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}