using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown;
using NoeticTools.Dashboard.Framework.Tiles.ServerStatus;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.Time;
using NoeticTools.TeamDashboard.Tiles.Message;

namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileRegistry : ITileRegistry
    {
        private readonly TeamCityService _teamCityService;
        private readonly IClock _clock;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly IList<ITileViewModel> _tileViewModels = new List<ITileViewModel>();

        public TileRegistry(TeamCityService teamCityService, IClock clock, ITimerService timerService, IDashboardController dashboardController)
        {
            _teamCityService = teamCityService;
            _clock = clock;
            _timerService = timerService;
            _dashboardController = dashboardController;
        }

        public ITileViewModel GetNew(DashboardTileConfiguration tileConfiguration)
        {
            var factoryLookup = new Dictionary<string, Func<DashboardTileConfiguration, ITileViewModel>>
            {
                {DateTileViewModel.TileTypeId, CreateDateTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B1".ToLower(), CreateDateTile},
                {DaysLeftCountDownViewModel.TileTypeId, CreateDaysLeftTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B2".ToLower(), CreateDaysLeftTile},
                {ServerStatusTileViewModel.TileTypeId, CreateServerStatusTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B4".ToLower(), CreateServerStatusTile},
                {TeamCityAvailableBuildsViewModel.TileTypeId, CreateTeamCityAvailableBuildsTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B6".ToLower(), CreateTeamCityAvailableBuildsTile},
                {TeamCityLastBuildStatusTileViewModel.TileTypeId, CreateTeamCityLastBuildTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B5".ToLower(), CreateTeamCityLastBuildTile},
                {MessageTileViewModel.TileTypeId, CreateMessageTile},
                {"0FFACE9A-8B68-4DBC-8B42-0255F51368B3".ToLower(), CreateMessageTile},
                {WebPageTileViewModel.TileTypeId, CreateWebPageTile},
                {"92CE0D61-4748-4427-8EB7-DC8B8B741C15".ToLower(), CreateWebPageTile},
            };

            var tileViewModel = factoryLookup[tileConfiguration.TypeId](tileConfiguration);
            _tileViewModels.Add(tileViewModel);
            return tileViewModel;
        }

        public ITileViewModel[] GetAll()
        {
            return _tileViewModels.ToArray();
        }

        public void Clear()
        {
            _tileViewModels.Clear();
        }

        private ITileViewModel CreateDateTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DateTileViewModel.TileTypeId;
            return new DateTileViewModel(_timerService, _clock);
        }

        private ITileViewModel CreateTeamCityLastBuildTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityLastBuildStatusTileViewModel.TileTypeId;
            return new TeamCityLastBuildStatusTileViewModel(_teamCityService, tileConfiguration, _timerService, _dashboardController);
        }

        private ITileViewModel CreateTeamCityAvailableBuildsTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityAvailableBuildsViewModel.TileTypeId;
            return new TeamCityAvailableBuildsViewModel(_teamCityService, tileConfiguration, _timerService, _dashboardController);
        }

        private ITileViewModel CreateServerStatusTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = ServerStatusTileViewModel.TileTypeId;
            return new ServerStatusTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateDaysLeftTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DaysLeftCountDownViewModel.TileTypeId;
            return new DaysLeftCountDownViewModel(tileConfiguration, _clock, _dashboardController);
        }

        private ITileViewModel CreateMessageTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = MessageTileViewModel.TileTypeId;
            return new MessageTileViewModel(tileConfiguration, _dashboardController);
        }

        private ITileViewModel CreateWebPageTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = WebPageTileViewModel.TileTypeId;
            return new WebPageTileViewModel(tileConfiguration, _dashboardController);
        }
    }
}