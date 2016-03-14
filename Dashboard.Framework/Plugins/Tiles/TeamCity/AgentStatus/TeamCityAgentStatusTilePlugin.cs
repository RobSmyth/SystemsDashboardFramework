using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;
        private IServices _services;
        private const string TileTypeId = "TeamCity.Agent.Status";

        public TeamCityAgentStatusTilePlugin(TeamCityService service, IDashboardController dashboardController)
        {
            _service = service;
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public string Name => "TeamCity build agent status (future)";

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new TeamCityAgentStatusTileControl();
            new TeamCityAgentStatusTileViewModel(_service, tile, _dashboardController, layoutController, _services, view, _service);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(this);
        }
    }
}