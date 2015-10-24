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
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.TeamDashboard.Tiles;
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

                {HelpTileViewModel.TileTypeId, CreateHelpTile},
                {"14B30D27-DC55-466A-A7C7-835004FF94A6".ToLower(), CreateHelpTile},

                {NavigationTileViewModel.TileTypeId, CreateNavigationTile},
                {"49D4EBB4-3C2E-44A6-AF42-FC9EDCDE9D95".ToLower(), CreateNavigationTile},
            };

            TileLayoutController layoutController = null;

            if (tileConfiguration.TypeId == DashboardTileConfiguration.BlankTileTypeId || 
                tileConfiguration.TypeId.Equals("6f1bf918-6080-42c2-b980-d562f77cb4e6", StringComparison.InvariantCultureIgnoreCase))
            {
                tileConfiguration.TypeId = DashboardTileConfiguration.BlankTileTypeId;
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
            tileConfiguration.TypeId = DateTileViewModel.TileTypeId;
            return new DateTileViewModel(tileConfiguration.Id);
        }

        private ITileViewModel CreateTeamCityLastBuildTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityLastBuildStatusTileViewModel.TileTypeId;
            return new TeamCityLastBuildStatusTileViewModel(_teamCityService, tileConfiguration);
        }

        private ITileViewModel CreateTeamCityAvailableBuildsTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = TeamCityAvailableBuildsViewModel.TileTypeId;
            return new TeamCityAvailableBuildsViewModel(_teamCityService, tileConfiguration);
        }

        private ITileViewModel CreateServerStatusTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = ServerStatusTileViewModel.TileTypeId;
            return new ServerStatusTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateDaysLeftTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = DaysLeftCountDownViewModel.TileTypeId;
            return new DaysLeftCountDownViewModel(tileConfiguration);
        }

        private ITileViewModel CreateMessageTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = MessageTileViewModel.TileTypeId;
            return new MessageTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateWebPageTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = WebPageTileViewModel.TileTypeId;
            return new WebPageTileViewModel(tileConfiguration);
        }

        private ITileViewModel CreateHelpTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = HelpTileViewModel.TileTypeId;
            return new HelpTileViewModel();
        }

        private ITileViewModel CreateNavigationTile(DashboardTileConfiguration tileConfiguration)
        {
            tileConfiguration.TypeId = NavigationTileViewModel.TileTypeId;
            return new NavigationTileViewModel();
        }
    }
}