using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.LastBuildStatus
{
    public sealed class TeamCityLastBuildStatusTileProvider : ITileControllerProvider
    {
        private readonly ITeamCityChannel _channel;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityLastBuildStatusTileProvider(ITeamCityChannel channel, IDashboardController dashboardController, IServices services)
        {
            _channel = channel;
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "TeamCity running or last build status";

        public bool MatchesId(string id)
        {
            return id == TeamCityLastBuildStatusTileViewModel.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B5", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new TeamCityBuildStatusTileControl();
            new TeamCityLastBuildStatusTileViewModel(_channel, tile, _dashboardController, layoutController, _services, view);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TeamCityLastBuildStatusTileViewModel.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}