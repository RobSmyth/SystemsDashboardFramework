using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown;
using NoeticTools.Dashboard.Framework.Tiles.Message;
using NoeticTools.Dashboard.Framework.Tiles.ServerStatus;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileRegistry : ITileRegistry
    {
        private readonly TeamCityService _teamCityService;
        private readonly IClock _clock;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly IEnumerable<ITilePlugin> _plugins;
        private readonly IList<IViewController> _tileControllers = new List<IViewController>();

        public TileRegistry(TeamCityService teamCityService, IClock clock, ITimerService timerService, IDashboardController dashboardController, IEnumerable<ITilePlugin> plugins)
        {
            _teamCityService = teamCityService;
            _clock = clock;
            _timerService = timerService;
            _dashboardController = dashboardController;
            _plugins = plugins;
        }

        public IViewController GetNew(TileConfiguration tileConfiguration)
        {
            var plugin = _plugins.First(x => x.MatchesId(tileConfiguration.TypeId));
            var tileViewModel = plugin.CreateViewController(tileConfiguration);
            _tileControllers.Add(tileViewModel);
            return tileViewModel;
        }

        public IViewController[] GetAll()
        {
            return _tileControllers.ToArray();
        }

        public void Clear()
        {
            _tileControllers.Clear();
        }
    }
}