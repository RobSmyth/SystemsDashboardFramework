using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown
{
    public sealed class DaysLeftCountDownTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "TimeLeft.Days.Count";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public DaysLeftCountDownTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Working days left to date count down";

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B2", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new DaysLeftCoundDownTileView();
            new DaysLeftCountDownTileViewModel(tile, _services.Clock, _dashboardController, view, layoutController, _services);
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