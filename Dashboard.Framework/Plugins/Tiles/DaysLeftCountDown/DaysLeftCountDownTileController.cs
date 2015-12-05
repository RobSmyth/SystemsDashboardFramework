using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown
{
    internal class DaysLeftCountDownTileController : IViewController
    {
        public const string TileTypeId = "TimeLeft.Days.Count";
        private readonly TileConfiguration _tileConfiguration;
        private readonly IClock _clock;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        private readonly IServices _services;

        public DaysLeftCountDownTileController(TileConfiguration tileConfiguration, IClock clock, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services)
        {
            _tileConfiguration = tileConfiguration;
            _clock = clock;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            var view = new DaysLeftCountDownTileView();
            new DaysLeftCountDownTileViewModel(_tileConfiguration, _clock, _dashboardController, view, _tileLayoutController, _services);
            return view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}