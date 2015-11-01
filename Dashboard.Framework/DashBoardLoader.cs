using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Time;

namespace NoeticTools.Dashboard.Framework
{
    public class DashBoardLoader : IDashBoardLoader
    {
        private readonly DashboardConfigurations _config;
        private readonly RunOptions _runOptions;
        private readonly Grid _tileGrid;
        private readonly IClock _clock;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;

        public DashBoardLoader(DashboardConfigurations config, RunOptions runOptions, Grid tileGrid, IClock clock, ITimerService timerService, IDashboardController dashboardController)
        {
            _config = config;
            _runOptions = runOptions;
            _tileGrid = tileGrid;
            _clock = clock;
            _timerService = timerService;
            _dashboardController = dashboardController;
        }

        public void Load(DashboardConfiguration configuration)
        {
            var channel = new TeamCityService(_config.Services, _runOptions);
            var layout = new TileLayoutController(_tileGrid, channel, configuration, _clock, _timerService, _dashboardController);
            layout.Clear();

            foreach (var tile in configuration.RootTile.Tiles)
            {
                layout.AddTile(tile);
            }
        }
    }
}