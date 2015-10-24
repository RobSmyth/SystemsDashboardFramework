using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dashboard.Config;
using Dashboard.Framework.Config;
using Dashboard.Tiles;
using Dashboard.Tiles.Date;
using Dashboard.Tiles.DaysLeftCountDown;
using Dashboard.Tiles.HelpTile;
using Dashboard.Tiles.Message;
using Dashboard.Tiles.NavigationTile;
using Dashboard.Tiles.ServerStatus;
using Dashboard.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.TeamDashboard.Tiles.TeamCityAvailableBuilds;
using NoeticTools.TeamDashboard.Tiles.TeamCityLastBuildStatus;

namespace NoeticTools.TeamDashboard
{
    public class TileLayoutController
    {
        private readonly DashboardConfiguration _activeConfiguration; //>>> move out when factories resolved
        private readonly TeamCityService _teamCityService; //>>> move out when factories resolved

        public TileLayoutController(Grid grid, TeamCityService teamCityService,
            DashboardConfiguration activeConfiguration)
        {
            _teamCityService = teamCityService;
            _activeConfiguration = activeConfiguration;
            Grid = grid;
        }

        public Grid Grid { get; private set; }

        private TileLayoutController AddTile(DashboardTileConfiguration tileConfiguration, ITileViewModel viewModel)
        {
            Grid panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            var layout = new TileLayoutController(panel, _teamCityService, _activeConfiguration)
            {
                Grid = {Margin = new Thickness(2)}
            };
            viewModel.Start(layout.Grid);
            return this;
        }

        private TileLayoutController AddPanel(DashboardTileConfiguration tileConfiguration)
        {
            Grid panel = AddPlaceholderPanel(tileConfiguration.RowNumber, tileConfiguration.ColumnNumber,
                tileConfiguration.RowSpan, tileConfiguration.ColumnSpan);
            var layout = new TileLayoutController(panel, _teamCityService, _activeConfiguration)
            {
                Grid = {Margin = new Thickness(2)}
            };
            return layout;
        }

        private Grid AddPlaceholderPanel(int rowNumber, int columnNumber, int rowSpan, int columnSpan)
        {
            while (rowNumber + rowSpan - 1 > Grid.RowDefinitions.Count)
            {
                Grid.RowDefinitions.Add(new RowDefinition());
            }

            while (columnNumber + columnSpan - 1 > Grid.ColumnDefinitions.Count)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var panel = new Grid {Margin = new Thickness(0)};

            Grid.SetRow(panel, rowNumber - 1);
            Grid.SetColumn(panel, columnNumber - 1);
            Grid.SetRowSpan(panel, rowSpan);
            Grid.SetColumnSpan(panel, columnSpan);

            Grid.Children.Add(panel);
            return panel;
        }

        public void AddTile(DashboardTileConfiguration tileConfiguration)
        {
            var factoryLookup = new Dictionary<Guid, Func<DashboardTileConfiguration, ITileViewModel>>
            {
                {DateTileViewModel.TileTypeId, CreateDateTile},
                {DaysLeftCountDownViewModel.TileTypeId, CreateDaysLeftTile},
                {ServerStatusTileViewModel.TileTypeId, CreateServerStatusTile},
                {TeamCityAvailableBuildsViewModel.TileTypeId, CreateTeamCityAvailableBuildsTile},
                {TeamCityLastBuildStatusTileViewModel.TileTypeId, CreateTeamCityLastBuildTile},
                {MessageTileViewModel.TileTypeId, CreateMessageTile},
                {WebPageTileViewModel.TileTypeId, CreateWebPageTile},
                {HelpTileViewModel.TileTypeId, CreateHelpTile},
                {NavigationTileViewModel.TileTypeId, CreateNavigationTile},
            };

            TileLayoutController layoutController = null;

            if (tileConfiguration.TypeId == DashboardTileConfiguration.BlankTileTypeId)
            {
                layoutController = AddPanel(tileConfiguration);
            }
            else
            {
                layoutController = AddTile(tileConfiguration, factoryLookup[tileConfiguration.TypeId](tileConfiguration));
            }

            foreach (DashboardTileConfiguration tile in tileConfiguration.Tiles)
            {
                layoutController.AddTile(tile);
            }
        }

        private ITileViewModel CreateDateTile(DashboardTileConfiguration tileConfiguration)
        {
            return new DateTileViewModel(tileConfiguration.Id);
        }

        private ITileViewModel CreateTeamCityLastBuildTile(DashboardTileConfiguration tileConfiguration)
        {
            return new TeamCityLastBuildStatusTileViewModel(_teamCityService, tileConfiguration);
        }

        private ITileViewModel CreateTeamCityAvailableBuildsTile(DashboardTileConfiguration tileConfiguration)
        {
            return new TeamCityAvailableBuildsViewModel(_teamCityService, tileConfiguration);
        }

        private ITileViewModel CreateServerStatusTile(DashboardTileConfiguration tileConfiguration)
        {
            return new ServerStatusTileViewModel(tileConfiguration); //>>>>
        }

        private ITileViewModel CreateDaysLeftTile(DashboardTileConfiguration tileConfiguration)
        {
            return new DaysLeftCountDownViewModel(tileConfiguration);
        }

        private ITileViewModel CreateMessageTile(DashboardTileConfiguration tileConfiguration)
        {
            return new MessageTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateWebPageTile(DashboardTileConfiguration tileConfiguration)
        {
            return new WebPageTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateHelpTile(DashboardTileConfiguration tileConfiguration)
        {
            return new HelpTileViewModel();
        }

        private ITileViewModel CreateNavigationTile(DashboardTileConfiguration tileConfiguration)
        {
            return new NavigationTileViewModel();
        }
    }
}