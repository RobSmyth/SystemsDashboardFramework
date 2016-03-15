using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Tiles.ServerStatus;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class WmiTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;
        private IServices _services;

        public WmiTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(new WmiTileProvider(_dashboardController, _services));
        }
    }
}