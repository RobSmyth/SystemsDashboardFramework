﻿using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.LastBuildStatus
{
    public sealed class TeamCityLastBuildStatusTileProvider : ITileControllerProvider
    {
        private readonly ITeamCityChannel _channel;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityLastBuildStatusTileProvider(ITeamCityChannel channel, IDashboardController dashboardController, IServices services)
        {
            // todo - should not be passing in channel here. Breaks having multiple team city services.
            _channel = channel;
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "TeamCity running or last build status";

        public string TypeId => TeamCityLastBuildStatusTileViewModel.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TeamCityLastBuildStatusTileViewModel.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B5", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var view = new TeamCityBuildStatusTileControl();
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new TeamCityLastBuildStatusTileViewModel(_channel, tileConfigturation, _dashboardController, layoutController, _services, view, tileProperties);
            conduit.SetTarget(viewModel);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TeamCityLastBuildStatusTileViewModel.TileTypeId,
            };
        }
    }
}