using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ExpiredTimeAlert
{
    public class ExpiredTimeAlertTilePlugin : IPlugin, ITileControllerProvider
    {
        private const string TileTypeId = "ExpiredTime.Alert";
        private readonly IDashboardController _dashboardController;
        private readonly IClock _clock;
        private readonly IServices _services;

        public ExpiredTimeAlertTilePlugin(IDashboardController dashboardController, IClock clock, IServices services)
        {
            _dashboardController = dashboardController;
            _clock = clock;
            _services = services;
        }

        public int Rank => 0;

        public string Name => "Expired time alert (ALPHA)";

        public bool MatchesId(string id)
        {
            return id.Equals(TileTypeId, StringComparison.InvariantCulture);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new ExpiredTimeAlert.ExpiredTimeAlertTileView();
            new ExpiredTimeAlertTileViewModel(tile, _clock, _dashboardController, view, layoutController, _services);
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

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }
    }
}