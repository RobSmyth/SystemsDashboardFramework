using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown
{
    internal class DaysLeftCountDownTileController : IViewController
    {
        public const string TileTypeId = "TimeLeft.Days.Count";
        private readonly TileConfiguration _tileConfiguration;
        private readonly IClock _clock;
        private readonly IDashboardController _dashboardController;
        private readonly ITimerService _timerService;

        public DaysLeftCountDownTileController(TileConfiguration tileConfiguration, IClock clock, IDashboardController dashboardController, ITimerService timerService)
        {
            _tileConfiguration = tileConfiguration;
            _clock = clock;
            _dashboardController = dashboardController;
            _timerService = timerService;
        }

        public FrameworkElement CreateView()
        {
            var view = new DaysLeftCountDownTileView();
            new DaysLeftCountDownTileViewModel(_tileConfiguration, _clock, _dashboardController, view, _timerService);
            return view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}