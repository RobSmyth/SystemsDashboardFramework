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
        private readonly IList<IViewController> _tileViewModels = new List<IViewController>();

        public TileRegistry(TeamCityService teamCityService, IClock clock, ITimerService timerService, IDashboardController dashboardController)
        {
            _teamCityService = teamCityService;
            _clock = clock;
            _timerService = timerService;
            _dashboardController = dashboardController;
        }

        public IViewController GetNew(TileConfiguration tileConfiguration)
        {
            var factoryLookup = new Dictionary<string, Func<TileConfiguration, IViewController>>
            {
                {DateViewController.TileTypeId, CreateDateTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B1".ToLower(), CreateDateTile},
                {DaysLeftCountDownViewController.TileTypeId, CreateDaysLeftTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B2".ToLower(), CreateDaysLeftTile},
                {ServerStatusViewController.TileTypeId, CreateServerStatusTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B4".ToLower(), CreateServerStatusTile},
                {TeamCityAvailableBuildsViewController.TileTypeId, CreateTeamCityAvailableBuildsTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B6".ToLower(), CreateTeamCityAvailableBuildsTile},
                {TeamCityLastBuildStatusViewController.TileTypeId, CreateTeamCityLastBuildTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B5".ToLower(), CreateTeamCityLastBuildTile},
                {MessageViewController.TileTypeId, CreateMessageTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B3".ToLower(), CreateMessageTile},
                {WebPageViewController.TileTypeId, CreateWebPageTile},
                {"92CE0D61-4748-4427-8EB7-DC8B8B741C15".ToLower(), CreateWebPageTile}
            };

            var tileViewModel = factoryLookup[tileConfiguration.TypeId](tileConfiguration);
            _tileViewModels.Add(tileViewModel);
            return tileViewModel;
        }

        public IViewController[] GetAll()
        {
            return _tileViewModels.ToArray();
        }

        public void Clear()
        {
            _tileViewModels.Clear();
        }

        private IViewController CreateDateTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DateViewController.TileTypeId;
            return new DateViewController(_timerService, _clock);
        }

        private IViewController CreateTeamCityLastBuildTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityLastBuildStatusViewController.TileTypeId;
            return new TeamCityLastBuildStatusViewController(_teamCityService, tileConfiguration, _timerService,
                _dashboardController);
        }

        private IViewController CreateTeamCityAvailableBuildsTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityAvailableBuildsViewController.TileTypeId;
            return new TeamCityAvailableBuildsViewController(_teamCityService, tileConfiguration, _timerService,
                _dashboardController);
        }

        private IViewController CreateServerStatusTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = ServerStatusViewController.TileTypeId;
            return new ServerStatusViewController(tileConfiguration);
        }

        private IViewController CreateDaysLeftTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DaysLeftCountDownViewController.TileTypeId;
            return new DaysLeftCountDownViewController(tileConfiguration, _clock, _dashboardController);
        }

        private IViewController CreateMessageTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = MessageViewController.TileTypeId;
            return new MessageViewController(tileConfiguration, _dashboardController);
        }

        private IViewController CreateWebPageTile(TileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = WebPageViewController.TileTypeId;
            return new WebPageViewController(tileConfiguration, _dashboardController);
        }
    }
}