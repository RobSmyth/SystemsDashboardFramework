using System;
using System.Windows;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityAvailbleBuildsTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private ILog _logger;

        public TeamCityAvailbleBuildsTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
            _logger = LogManager.GetLogger("Plugin.TeamCity.AvailableBuilds");
        }

        public string Name => "TeamCity available builds";

        public string TypeId => TeamCityAvailableBuildsTileViewModel.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TeamCityAvailableBuildsTileViewModel.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B6", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var teamCityService = _services.GetService<ITeamCityService>("TeamCity");
            var view = new TeamCityAvailableBuildsListControl();
            new TeamCityAvailableBuildsTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, view, teamCityService);
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