using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ExpiredTimeAlert
{
    public class ExpiredTimeAlertTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "ExpiredTime.Alert";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public ExpiredTimeAlertTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Expired time alert (ALPHA)";

        public bool MatchesId(string id)
        {
            return id.Equals(TileTypeId, StringComparison.InvariantCulture);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new ExpiredTimeAlert.ExpiredTimeAlertTileView();
            new ExpiredTimeAlertTileViewModel(tile, _services.Clock, _dashboardController, view, layoutController, _services);
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