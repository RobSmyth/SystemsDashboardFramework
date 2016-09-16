﻿using log4net;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityLAvailbleBuildSTilePlugin : IPlugin
    {
        private ILog _logger;

        public TeamCityLAvailbleBuildSTilePlugin()
        {
            _logger = LogManager.GetLogger("Plugin.TeamCity.AvailableBuilds");
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityAvailbleBuildsTileProvider(services.DashboardController, services));
        }
    }
}