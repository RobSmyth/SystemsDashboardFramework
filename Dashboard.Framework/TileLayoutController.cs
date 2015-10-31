using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dashboard.Tiles.Date;
using Dashboard.Tiles.ServerStatus;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;
using NoeticTools.TeamDashboard.Tiles;
using NoeticTools.TeamDashboard.Tiles.Date;
using NoeticTools.TeamDashboard.Tiles.DaysLeftCountDown;
using NoeticTools.TeamDashboard.Tiles.Message;
using NoeticTools.TeamDashboard.Tiles.TeamCityAvailableBuilds;
using NoeticTools.TeamDashboard.Tiles.TeamCityLastBuildStatus;
using NoeticTools.TeamDashboard.Tiles.WebPage;

namespace NoeticTools.TeamDashboard
{
    public class TileLayoutController : ITileLayoutController
    {
        private readonly DashboardConfiguration _activeConfiguration; //>>> move out when factories resolved
        private readonly IClock _clock;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly TeamCityService _teamCityService; //>>> move out when factories resolved
        private readonly Grid _tileGrid;

        public TileLayoutController(Grid tileGrid, TeamCityService teamCityService,
            DashboardConfiguration activeConfiguration, IClock clock, ITimerService timerService, IDashboardController dashboardController)
        {
            _teamCityService = teamCityService;
            _activeConfiguration = activeConfiguration;
            _clock = clock;
            _timerService = timerService;
            _dashboardController = dashboardController;
            _tileGrid = tileGrid;
        }

        private TileLayoutController AddTile(DashboardTileConfiguration tileConfiguration, ITileViewModel viewModel)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            var layout = new TileLayoutController(panel, _teamCityService, _activeConfiguration, _clock, _timerService, _dashboardController)
            {
                _tileGrid = {Margin = new Thickness(2)}
            };
            var view = viewModel.CreateView();
            panel.Children.Add(view);
            return this;
        }

        private TileLayoutController AddPanel(DashboardTileConfiguration tileConfiguration)
        {
            var panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            var layout = new TileLayoutController(panel, _teamCityService, _activeConfiguration, _clock, _timerService, _dashboardController)
            {
                _tileGrid = {Margin = new Thickness(2)}
            };
            return layout;
        }

        private Grid AddPlaceholderPanel(int rowNumber, int columnNumber, int rowSpan, int columnSpan)
        {
            while (rowNumber + rowSpan - 1 > _tileGrid.RowDefinitions.Count)
            {
                _tileGrid.RowDefinitions.Add(new RowDefinition());
            }

            while (columnNumber + columnSpan - 1 > _tileGrid.ColumnDefinitions.Count)
            {
                _tileGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var panel = new Grid {Margin = new Thickness(0)};

            Grid.SetRow(panel, rowNumber - 1);
            Grid.SetColumn(panel, columnNumber - 1);
            Grid.SetRowSpan(panel, rowSpan);
            Grid.SetColumnSpan(panel, columnSpan);

            _tileGrid.Children.Add(panel);
            return panel;
        }

        public void AddTile(DashboardTileConfiguration tileConfiguration)
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

            TileLayoutController layoutController = null;

            if (tileConfiguration.TypeId == DashboardTileConfiguration.BlankTileTypeId ||
                tileConfiguration.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                tileConfiguration.TypeId = DashboardTileConfiguration.BlankTileTypeId;
                layoutController = AddPanel(tileConfiguration);
            }
            else
            {
                layoutController = AddTile(tileConfiguration, factoryLookup[tileConfiguration.TypeId](tileConfiguration));
            }

            foreach (var tile in tileConfiguration.Tiles)
            {
                layoutController.AddTile(tile);
            }
        }

        private ITileViewModel CreateDateTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DateTileViewModel.TileTypeId;
            return new DateTileViewModel();
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