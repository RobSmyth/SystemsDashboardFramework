using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown
{
    public class DaysLeftCountDownTilePlugin : IPlugin, ITileControllerProvider
    {
        private const string TileTypeId = "TimeLeft.Days.Count";
        private readonly IDashboardController _dashboardController;
        private readonly IClock _clock;
        private readonly IServices _services;

        public DaysLeftCountDownTilePlugin(IDashboardController dashboardController, IClock clock, IServices services)
        {
            _dashboardController = dashboardController;
            _clock = clock;
            _services = services;
        }

        public int Rank => 0;

        public string Name => "Working days left to date count down";

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B2", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new DaysLeftCountDownTileView();
            new DaysLeftCountDownTileViewModel(tile, _clock, _dashboardController, view, layoutController, _services);
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