using System;
using System.Windows;
using log4net;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityLAvailbleBuildSTileProvider : ITileControllerProvider
    {
        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private ILog _logger;

        public TeamCityLAvailbleBuildSTileProvider(TeamCityService service, IDashboardController dashboardController, IServices services)
        {
            _service = service;
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
            var view = new TeamCityAvailableBuildsListControl();
            new TeamCityAvailableBuildsTileViewModel(_service, tile, _dashboardController, layoutController, _services, view);
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