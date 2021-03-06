﻿using log4net;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityAvailbleBuildSTilePlugin : IPlugin
    {
        private readonly string _serviceName;
        private ILog _logger;

        public TeamCityAvailbleBuildSTilePlugin(string serviceName)
        {
            _serviceName = serviceName;
            _logger = LogManager.GetLogger($"Plugin.{serviceName}.AvailableBuilds");
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityAvailbleBuildsTileProvider(services.DashboardController, services, _serviceName));
        }
    }
}