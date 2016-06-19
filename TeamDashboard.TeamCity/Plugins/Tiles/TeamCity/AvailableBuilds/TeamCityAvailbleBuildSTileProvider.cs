using System;
using System.Windows;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityAvailbleBuildsTileProvider : ITileControllerProvider
    {
        private readonly ITeamCityChannel _channel;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private ILog _logger;

        public TeamCityAvailbleBuildsTileProvider(ITeamCityChannel channel, IDashboardController dashboardController, IServices services)
        {
            _channel = channel;
            _dashboardController = dashboardController;
            _services = services;
            _logger = LogManager.GetLogger("Plugin.TeamCity.AvailableBuilds");
        }

        public string Name => "TeamCity available builds";

        public bool MatchesId(string id)
        {
            return id == TeamCityAvailableBuildsTileViewModel.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B6", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var teamCityService = _services.GetService<ITeamCityService>("TeamCity");
            var view = new TeamCityAvailableBuildsListControl();
            new TeamCityAvailableBuildsTileViewModel(tile, _dashboardController, layoutController, _services, view, teamCityService);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TeamCityAvailableBuildsTileViewModel.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}